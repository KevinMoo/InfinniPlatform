﻿using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Writers.Pdf
{
    internal static class ProcessHelper
    {
        public static async Task<ProcessResult> ExecuteShellCommand(string command, string arguments, int timeout)
        {
            var result = new ProcessResult();

            using (var process = new Process())
            {
                // При запуске на Linux bash-скриптов, возможен код ошибки 255.
                // Решением является добавление заголовка #!/bin/bash в начало скрипта.

                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                // Подписка на события записи в выходные потоки процесса

                var outputBuilder = new StringBuilder();
                var outputCloseEvent = new TaskCompletionSource<bool>();

                process.OutputDataReceived += (s, e) =>
                                              {
                                                  // Поток output закрылся (процесс завершил работу)
                                                  if (string.IsNullOrEmpty(e.Data))
                                                  {
                                                      outputCloseEvent.SetResult(true);
                                                  }
                                                  else
                                                  {
                                                      outputBuilder.AppendLine(e.Data);
                                                  }
                                              };

                var errorBuilder = new StringBuilder();
                var errorCloseEvent = new TaskCompletionSource<bool>();

                process.ErrorDataReceived += (s, e) =>
                                             {
                                                 // Поток error закрылся (процесс завершил работу)
                                                 if (string.IsNullOrEmpty(e.Data))
                                                 {
                                                     errorCloseEvent.SetResult(true);
                                                 }
                                                 else
                                                 {
                                                     errorBuilder.AppendLine(e.Data);
                                                 }
                                             };

                bool isStarted;

                try
                {
                    isStarted = process.Start();
                }
                catch (Exception error)
                {
                    // Usually it occurs when an executable file is not found or is not executable

                    result.Completed = true;
                    result.ExitCode = -1;
                    result.Output = string.Format(Resources.CannotExecuteCommand, command, arguments, error.Message);

                    isStarted = false;
                }

                if (isStarted)
                {
                    // Reads the output stream first and then waits because deadlocks are possible
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Creates task to wait for process exit using timeout
                    var waitForExit = WaitForExitAsync(process, timeout);

                    // Create task to wait for process exit and closing all output streams
                    var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);

                    // Waits process completion and then checks it was not completed by timeout
                    if (await Task.WhenAny(Task.Delay(timeout), processTask) == processTask && waitForExit.Result)
                    {
                        result.Completed = true;
                        result.ExitCode = process.ExitCode;

                        // Adds process output if it was completed with error
                        if (process.ExitCode != 0)
                        {
                            result.Output = $"{outputBuilder}{errorBuilder}";
                        }
                    }
                    else
                    {
                        try
                        {
                            // Kill hung process
                            process.Kill();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }

            return result;
        }


        private static Task<bool> WaitForExitAsync(Process process, int timeout)
        {
            return Task.Run(() => process.WaitForExit(timeout));
        }


        public struct ProcessResult
        {
            public bool Completed;
            public int? ExitCode;
            public string Output;
        }
    }
}