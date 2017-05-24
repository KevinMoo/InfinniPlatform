﻿using System.Text;

using InfinniPlatform.Diagnostics;
using InfinniPlatform.Http;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Security;
using InfinniPlatform.Serialization;
using InfinniPlatform.Session;

namespace InfinniPlatform.IoC
{
    public class CoreContainerModule : IContainerModule
    {
        public CoreContainerModule(AppOptions options)
        {
            _options = options;
        }

        private readonly AppOptions _options;

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf();

            RegisterDiagnosticsComponents(builder);
            RegisterSerializationComponents(builder);
            RegisterSecurityComponents(builder);
            RegisterSessionComponents(builder);
            RegisterHttpComponents(builder);
        }


        private void RegisterDiagnosticsComponents(IContainerBuilder builder)
        {
            builder.RegisterType<SystemInfoHttpService>()
                   .As<IHttpService>()
                   .SingleInstance();
        }

        private void RegisterSerializationComponents(IContainerBuilder builder)
        {
            builder.RegisterInstance(JsonObjectSerializer.DefaultEncoding)
                   .As<Encoding>()
                   .SingleInstance();

            builder.RegisterType<JsonObjectSerializer>()
                   .As<IObjectSerializer>()
                   .As<IJsonObjectSerializer>()
                   .SingleInstance();
        }

        private void RegisterSecurityComponents(IContainerBuilder builder)
        {
            builder.RegisterType<UserIdentityProvider>()
                   .As<IUserIdentityProvider>()
                   .SingleInstance();
        }

        private void RegisterSessionComponents(IContainerBuilder builder)
        {
            builder.RegisterType<TenantScopeProvider>()
                   .As<ITenantScopeProvider>()
                   .SingleInstance();

            builder.RegisterType<TenantProvider>()
                   .As<ITenantProvider>()
                   .SingleInstance();
        }

        private void RegisterHttpComponents(IContainerBuilder builder)
        {
            // Hosting

            builder.RegisterType<HostAddressParser>()
                   .As<IHostAddressParser>()
                   .SingleInstance();

            // Middlewares

            builder.RegisterType<HttpContextProvider>()
                   .As<IHttpContextProvider>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<NancyHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // Services

            builder.RegisterType<HttpServiceNancyBootstrapper>()
                   .As<Nancy.Bootstrapper.INancyBootstrapper>()
                   .SingleInstance();

            builder.RegisterType<HttpServiceNancyModuleCatalog>()
                   .As<Nancy.INancyModuleCatalog>()
                   .SingleInstance();

            builder.RegisterGeneric(typeof(HttpServiceNancyModule<>))
                   .As(typeof(HttpServiceNancyModule<>))
                   .InstancePerDependency();

            builder.RegisterType<HttpServiceNancyModuleInitializer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<NancyMimeTypeResolver>()
                   .As<IMimeTypeResolver>()
                   .SingleInstance();

            builder.RegisterType<HttpRequestExcutorFactory>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<HttpServiceSource>()
                   .As<IHttpServiceSource>()
                   .SingleInstance();

            builder.RegisterType<HttpServiceContext>()
                   .As<IHttpServiceContext>()
                   .InstancePerDependency();

            builder.RegisterType<HttpServiceContextProvider>()
                   .As<IHttpServiceContextProvider>()
                   .SingleInstance();
        }
    }
}