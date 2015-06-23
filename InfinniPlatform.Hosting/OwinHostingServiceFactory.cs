﻿using System.Collections.Generic;
using System.Reflection;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Authentication.Modules;
using InfinniPlatform.Cors;
using InfinniPlatform.Factories;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Logging;
using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.SignalR;
using InfinniPlatform.SystemInfo;
using InfinniPlatform.WebApi;
using InfinniPlatform.WebApi.Factories;

namespace InfinniPlatform.Hosting
{
    /// <summary>
    ///     Фабрика для создания сервиса хостинга на базе OWIN (Open Web Interface for .NET).
    /// </summary>
    public sealed class OwinHostingServiceFactory : IHostingServiceFactory
    {
        private readonly OwinHostingService _hostingService;
        private readonly WebApiOwinHostingModule _webApiOwinHostingModule;

        public OwinHostingServiceFactory(IEnumerable<Assembly> assemblies,
            IServiceTemplateConfiguration serviceTemplateConfiguration = null, HostingConfig hostingConfig = null)
        {
            if (hostingConfig == null)
            {
                hostingConfig = HostingConfig.Default;
            }

            ControllerRoutingFactory.Instance = new ControllerRoutingFactory(hostingConfig);

            _hostingService = new OwinHostingService(config => config.Configuration(hostingConfig));

            // Error Handling
            _hostingService.RegisterModule(new UnhandledExceptionOwinHostingModule(Logger.Log));

            // ASP.NET CORS
            _hostingService.RegisterModule(new CorsOwinHostingModule());

            // System Info
            _hostingService.RegisterModule(new SystemInfoOwinHostingModule(new SystemInfoProvider()));

            // Authentication
            _hostingService.RegisterModule(new AuthenticationOwinHostingModule(GetExternalAuthenticationProviders()));

            // ASP.NET SignalR
            _hostingService.RegisterModule(new SignalROwinHostingModule());

            // Хостинг проприетарного протокола REST
            _hostingService.RegisterModule(new ApplicationHostingModule());

            // ASP.NET Web API
            serviceTemplateConfiguration = serviceTemplateConfiguration ?? new ServiceTemplateConfiguration();
            _webApiOwinHostingModule = new WebApiOwinHostingModule(assemblies,
                new ServiceRegistrationContainerFactory(serviceTemplateConfiguration), serviceTemplateConfiguration);
            _hostingService.RegisterModule(_webApiOwinHostingModule);
        }

        public InfinniPlatformHostServer InfinniPlatformHostServer
        {
            get { return _webApiOwinHostingModule.HostServer; }
        }

        /// <summary>
        ///     Создать сервис хостинга.
        /// </summary>
        public IHostingService CreateHostingService()
        {
            return _hostingService;
        }

        private static IEnumerable<OwinHostingModule> GetExternalAuthenticationProviders()
        {
            // Todo: Нужно подумать, как автоматизировать этот процесс

            var result = new List<OwinHostingModule>();

            if (AppSettings.GetValue("AppServerAuthAdfsEnable", false))
            {
                var server = AppSettings.GetValue("AppServerAuthAdfsServer");
                result.Add(new AuthenticationAdfsOwinHostingModule(server));
            }

            if (AppSettings.GetValue("AppServerAuthGoogleEnable", false))
            {
                var clientId = AppSettings.GetValue("AppServerAuthGoogleClientId");
                var clientSecret = AppSettings.GetValue("AppServerAuthGoogleClientSecret");
                result.Add(new AuthenticationGoogleOwinHostingModule(clientId, clientSecret));
            }

            if (AppSettings.GetValue("AppServerAuthEsiaEnable", false))
            {
                var server = AppSettings.GetValue("AppServerAuthEsiaServer");
                var clientId = AppSettings.GetValue("AppServerAuthEsiaClientId");
                var clientSecret = AppSettings.GetValue("AppServerAuthEsiaClientSecret");
                result.Add(new AuthenticationEsiaOwinHostingModule(server, clientId, clientSecret));
            }

            return result;
        }
    }
}