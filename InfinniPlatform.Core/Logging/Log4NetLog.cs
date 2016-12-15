﻿using System;
using System.Collections.Generic;
using System.Security.Principal;

using InfinniPlatform.Auth.Internal.Contract;
using InfinniPlatform.Sdk.Serialization;

using log4net;

using ILog = InfinniPlatform.Sdk.Logging.ILog;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Сервис <see cref="Sdk.Logging.ILog" /> на базе log4net.
    /// </summary>
    internal sealed class Log4NetLog : ILog
    {
        private const string KeyRequestId = "app.RequestId";
        private const string KeyUserId = "app.UserId";
        private const string KeyUserName = "app.UserName";


        public Log4NetLog(log4net.ILog internalLog, IJsonObjectSerializer serializer)
        {
            _internalLog = internalLog;
            _serializer = serializer;
        }


        private readonly log4net.ILog _internalLog;
        private readonly IJsonObjectSerializer _serializer;


        public bool IsDebugEnabled => _internalLog.IsDebugEnabled;

        public bool IsInfoEnabled => _internalLog.IsInfoEnabled;

        public bool IsWarnEnabled => _internalLog.IsWarnEnabled;

        public bool IsErrorEnabled => _internalLog.IsErrorEnabled;

        public bool IsFatalEnabled => _internalLog.IsFatalEnabled;


        public void Debug(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Debug(new LogEvent(message, exception, context, _serializer));
        }

        public void Info(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Info(new LogEvent(message, exception, context, _serializer));
        }

        public void Warn(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Warn(new LogEvent(message, exception, context, _serializer));
        }

        public void Error(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Error(new LogEvent(message, exception, context, _serializer));
        }

        public void Fatal(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Fatal(new LogEvent(message, exception, context, _serializer));
        }


        public void SetRequestId(object requestId)
        {
            LogicalThreadContext.Properties[KeyRequestId] = requestId;
        }

        public void SetUserId(IIdentity user)
        {
            if (user != null)
            {
                LogicalThreadContext.Properties[KeyUserId] = user.GetUserId();
                LogicalThreadContext.Properties[KeyUserName] = user.Name;
            }
            else
            {
                LogicalThreadContext.Properties[KeyUserId] = null;
                LogicalThreadContext.Properties[KeyUserName] = null;
            }
        }
    }
}