﻿using System;
using System.Collections.Generic;
using System.Security.Principal;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Сервис <see cref="ILog" /> на базе log4net.
    /// </summary>
    internal sealed class Log4NetLog : ILog
    {
        public Log4NetLog(log4net.ILog internalLog)
        {
            _internalLog = internalLog;
        }


        private readonly log4net.ILog _internalLog;


        public bool IsDebugEnabled => _internalLog.IsDebugEnabled;

        public bool IsInfoEnabled => _internalLog.IsInfoEnabled;

        public bool IsWarnEnabled => _internalLog.IsWarnEnabled;

        public bool IsErrorEnabled => _internalLog.IsErrorEnabled;

        public bool IsFatalEnabled => _internalLog.IsFatalEnabled;


        public void Debug(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _internalLog.Debug(new JsonEvent(message, context, exception));
        }

        public void Info(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _internalLog.Info(new JsonEvent(message, context, exception));
        }

        public void Warn(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _internalLog.Warn(new JsonEvent(message, context, exception));
        }

        public void Error(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _internalLog.Error(new JsonEvent(message, context, exception));
        }

        public void Fatal(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _internalLog.Fatal(new JsonEvent(message, context, exception));
        }


        public void InitThreadLoggingContext(IIdentity user, IDictionary<string, object> context)
        {
            log4net.ThreadContext.Properties.Clear();

            foreach (var pair in context)
            {
                log4net.ThreadContext.Properties[pair.Key] = pair.Value;
            }

            if (user != null)
            {
                log4net.ThreadContext.Properties["app.UserId"] = user.GetUserId();
                log4net.ThreadContext.Properties["app.UserName"] = user.Name;
            }
        }
    }
}