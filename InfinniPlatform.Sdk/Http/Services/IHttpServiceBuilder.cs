﻿using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Http.Services
{
    /// <summary>
    /// Регистратор обработчиков запросов сервиса.
    /// </summary>
    public interface IHttpServiceBuilder
    {
        /// <summary>
        /// Базовый путь сервиса.
        /// </summary>
        string ServicePath { get; set; }


        /// <summary>
        /// Правила обработки GET-запросов.
        /// </summary>
        IHttpServiceRouteBuilder Get { get; }

        /// <summary>
        /// Правила обработки POST-запросов.
        /// </summary>
        IHttpServiceRouteBuilder Post { get; }

        /// <summary>
        /// Правила обработки PUT-запросов.
        /// </summary>
        IHttpServiceRouteBuilder Put { get; }

        /// <summary>
        /// Правила обработки PATCH-запросов.
        /// </summary>
        IHttpServiceRouteBuilder Patch { get; }

        /// <summary>
        /// Правила обработки DELETE-запросов.
        /// </summary>
        IHttpServiceRouteBuilder Delete { get; }


        /// <summary>
        /// Предобработчик запросов.
        /// </summary>
        Func<IHttpRequest, Task<object>> OnBefore { get; set; }

        /// <summary>
        /// Постобработчик запросов.
        /// </summary>
        Func<IHttpRequest, object, Task<object>> OnAfter { get; set; }

        /// <summary>
        /// Обработчик исключений.
        /// </summary>
        Func<IHttpRequest, Exception, Task<object>> OnError { get; set; }


        /// <summary>
        /// Конвертер результата.
        /// </summary>
        Func<object, IHttpResponse> ResultConverter { get; set; }
    }
}