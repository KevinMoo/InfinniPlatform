﻿using System;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.UserInterface.Services.Metadata;

namespace InfinniPlatform.UserInterface.AppHost
{
    /// <summary>
    ///     Статические метаданные приложения.
    /// </summary>
    internal static class StaticMetadata
    {      
        /// <summary>
        ///     Создать метаданные главного окна приложения.
        /// </summary>
        public static dynamic CreateAppView(string server, int port)
        {
            var configId = AppSettings.GetValue("ConfigId");
            var viewMetadataService = new ViewMetadataService(null, configId, "Common",server,port);
            return viewMetadataService.GetItem("App");
        }

        /// <summary>
        ///     Сгенерировать версию конфигурации
        /// </summary>
        /// <returns></returns>
        public static string CreateVersion()
        {
            return "1.0.0.0"; //Guid.NewGuid().ToString();
        }
    }
}