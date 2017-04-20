﻿using System;

namespace InfinniPlatform.Http.Services
{
    /// <summary>
    /// Расширение NancyBootstrapper для подключения рендеринга Razor-представлений.
    /// </summary>
    public interface IViewEngineBootstrapperExtension
    {
        /// <summary>
        /// Тип локатора Razor-представлений.
        /// </summary>
        Type ViewLocatorType { get; }

        /// <summary>
        /// Регистрация источников Razor-представлений.
        /// </summary>
        /// <param name="nancyConventions">Соглашения Nancy.</param>
        void RegisterViewLocators(dynamic nancyConventions);
    }
}