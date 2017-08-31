﻿using InfinniPlatform.Http;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;

namespace InfinniPlatform.ServiceHost
{
    public class ContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Register dependencies
            builder.RegisterType<HttpService>()
                   .As<IHttpService>()
                   .SingleInstance();

            builder.RegisterType<LogContextLayer>()
                   .As<IDefaultAppLayer>()
                   .SingleInstance();
        }
    }
}