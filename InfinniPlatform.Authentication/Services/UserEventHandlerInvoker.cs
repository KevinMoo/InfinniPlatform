﻿using System;
using System.Collections.Generic;
using System.Security.Principal;

using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.Authentication.Services
{
    /// <summary>
    /// Предоставляет методы для вызова зарегистрированных обработчиков событий пользователя.
    /// </summary>
    internal class UserEventHandlerInvoker
    {
        public UserEventHandlerInvoker(IEnumerable<IUserEventHandler> userEventHandlers, ILog log)
        {
            _userEventHandlers = userEventHandlers;
            _log = log;
        }


        private readonly IEnumerable<IUserEventHandler> _userEventHandlers;
        private readonly ILog _log;


        /// <summary>
        /// Вызывается после входа пользователя в систему.
        /// </summary>
        /// <param name="identity">Идентификационные данные пользователя.</param>
        public async void OnAfterSignIn(IIdentity identity)
        {
            if (identity != null)
            {
                foreach (var userEventHandler in _userEventHandlers)
                {
                    try
                    {
                        await userEventHandler.OnAfterSignIn(identity);
                    }
                    catch (Exception exception)
                    {
                        // Исключения игнорируются, так как они не должны нарушить работоспособность основного механизма

                        _log.Error(string.Format(Resources.HandlingUserEventCompletedWithException, nameof(userEventHandler.OnAfterSignIn)), null, exception);
                    }
                }
            }
        }


        /// <summary>
        /// Вызывается перед выходом пользователя из системы.
        /// </summary>
        /// <param name="identity">Идентификационные данные пользователя.</param>
        public async void OnBeforeSignOut(IIdentity identity)
        {
            if (identity != null)
            {
                foreach (var userEventHandler in _userEventHandlers)
                {
                    try
                    {
                        await userEventHandler.OnBeforeSignOut(identity);
                    }
                    catch (Exception exception)
                    {
                        // Исключения игнорируются, так как они не должны нарушить работоспособность основного механизма

                        _log.Error(string.Format(Resources.HandlingUserEventCompletedWithException, nameof(userEventHandler.OnBeforeSignOut)), null, exception);
                    }
                }
            }
        }
    }
}