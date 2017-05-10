﻿using System;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.Auth;
using InfinniPlatform.Auth.Identity;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.ServiceHost
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("AppConfig.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"AppConfig.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }


        private readonly IConfigurationRoot _configuration;


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configureServices = services.AddLog4NetLogging()
                                            .AddAuthInternal<AppUser, AppUserRole>(_configuration, AuthCallback)
                                            .AddInMemoryCache()
                                            .AddRedisSharedCache(_configuration)
                                            .AddTwoLayerCache()
                                            .AddFileSystemBlobStorage(_configuration)
                                            .AddBlobStorageHttpService()
                                            .AddMongoDocumentStorage(_configuration)
                                            .AddDocumentStorageHttpService()
                                            .AddRabbitMqMessageQueue(_configuration)
                                            .AddQuartzScheduler(_configuration)
                                            .AddSchedulerHttpService()
                                            .AddPrintView(_configuration)
                                            .AddHeartbeatHttpService()
                                            .BuildProvider(_configuration);

            return configureServices;
        }

        private void AuthCallback(AuthInternalOptions opt)
        {
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            app.UseInfinniMiddlewares(resolver);
        }
    }
}