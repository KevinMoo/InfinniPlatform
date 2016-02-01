﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Пробрасывает сообщения из <see cref="Trace"/> в <see cref="ILog"/>.
    /// </summary>
    internal sealed class LogTraceListener : TraceListener
    {
        public LogTraceListener(ILog log)
        {
            _log = log;
        }


        private readonly ILog _log;


        public override bool IsThreadSafe => true;


        public override void Fail(string message)
        {
            _log.Error(message);
        }

        public override void Fail(string message, string detailMessage)
        {
            _log.Error(message + ' ' + message);
        }


        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            InternalLog(eventType, () =>
                                   {
                                       var builder = new StringBuilder();
                                       WriteHeader(builder, source, eventType, id);
                                       builder.Append(data).AppendLine();
                                       WriteFooter(builder, eventCache);
                                       return builder.ToString();
                                   });
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            InternalLog(eventType, () =>
                                   {
                                       var builder = new StringBuilder();
                                       WriteHeader(builder, source, eventType, id);
                                       builder.Append((data != null) ? string.Join(", ", data) : null).AppendLine();
                                       WriteFooter(builder, eventCache);
                                       return builder.ToString();
                                   });
        }


        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            TraceEvent(eventCache, source, TraceEventType.Transfer, id, message + ", relatedActivityId=" + relatedActivityId);
        }


        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            TraceEvent(eventCache, source, eventType, id, string.Empty);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
            {
                return;
            }

            InternalLog(eventType, () =>
                                   {
                                       var builder = new StringBuilder();
                                       WriteHeader(builder, source, eventType, id);
                                       builder.AppendLine(message);
                                       WriteFooter(builder, eventCache);
                                       return builder.ToString();
                                   });
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
            {
                return;
            }

            InternalLog(eventType, () =>
                                   {
                                       var builder = new StringBuilder();
                                       WriteHeader(builder, source, eventType, id);
                                       builder.AppendFormat(format, args).AppendLine();
                                       WriteFooter(builder, eventCache);
                                       return builder.ToString();
                                   });
        }


        public override void Write(string message)
        {
            _log.Debug(message);
        }

        public override void WriteLine(string message)
        {
            _log.Debug(message);
        }

        protected override void WriteIndent()
        {
        }


        private bool IsEnabled(TraceOptions opts)
        {
            return (opts & TraceOutputOptions) != 0;
        }

        private static void WriteHeader(StringBuilder message, string source, TraceEventType eventType, int id)
        {
            message.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}: {2} : ", source, eventType, id);
        }

        private void WriteFooter(StringBuilder message, TraceEventCache eventCache)
        {
            if (eventCache == null)
            {
                return;
            }

            if (IsEnabled(TraceOptions.ProcessId))
            {
                message.AppendLine("ProcessId=" + eventCache.ProcessId);
            }

            if (IsEnabled(TraceOptions.LogicalOperationStack))
            {
                message.Append("LogicalOperationStack=");

                var first = true;
                var operationStack = eventCache.LogicalOperationStack;

                foreach (var obj in operationStack)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        message.Append(", ");
                    }

                    message.Append(obj);
                }

                message.AppendLine();
            }

            if (IsEnabled(TraceOptions.ThreadId))
            {
                message.AppendLine("ThreadId=" + eventCache.ThreadId);
            }

            if (IsEnabled(TraceOptions.DateTime))
            {
                message.AppendLine("DateTime=" + eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
            }

            if (IsEnabled(TraceOptions.Timestamp))
            {
                message.AppendLine("Timestamp=" + eventCache.Timestamp);
            }

            if (IsEnabled(TraceOptions.Callstack))
            {
                message.AppendLine("Callstack=" + eventCache.Callstack);
            }
        }


        private void InternalLog(TraceEventType eventType, Func<string> message)
        {
            var logObject = new LazyLogObject(message);

            switch (eventType)
            {
                case TraceEventType.Critical:
                    _log.Fatal(logObject);
                    break;
                case TraceEventType.Error:
                    _log.Error(logObject);
                    break;
                case TraceEventType.Warning:
                    _log.Warn(logObject);
                    break;
                case TraceEventType.Information:
                    _log.Info(logObject);
                    break;
                default:
                    _log.Debug(logObject);
                    break;
            }
        }


        private sealed class LazyLogObject
        {
            public LazyLogObject(Func<string> toString)
            {
                _toString = toString;
            }

            private readonly Func<string> _toString;

            public override string ToString()
            {
                return _toString();
            }
        }
    }
}