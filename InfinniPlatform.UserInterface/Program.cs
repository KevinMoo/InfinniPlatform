﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.UserInterface.AppHost;

namespace InfinniPlatform.UserInterface
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                var hostProcesses = Process.GetProcessesByName("InfinniPlatform.RestfulApi");

                if (hostProcesses.Length == 0)
                {
                    TestApi.StartServer(p => p.SetHostingConfig(HostingConfig.Default));
                }
            }
#endif
            try
            {
                new SignInApi().SignInInternal(AuthorizationStorageExtensions.AdminUser,
                    AppSettings.GetValue("AdminPassword", "Admin"), true);
            }
            catch
            {
                //не удалось авторизоваться
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var appViewMetadata = StaticMetadata.CreateAppView();
            AppRunner.Run(appViewMetadata);
        }
    }
}