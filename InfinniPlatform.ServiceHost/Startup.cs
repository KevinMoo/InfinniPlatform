﻿using System;

using InfinniPlatform.Extensions;
using InfinniPlatform.AspNetCore;
using InfinniPlatform.Core.Abstractions.IoC;

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
            var configureServices = services.AddAuth()
                                            .AddCaching()
                                            .AddFileSystemBlobStorage(_configuration)
                                            .AddBlobStorageHttpService()
                                            .AddMongoDocumentStorage(_configuration)
                                            .AddDocumentStorageHttpService()
                                            .AddLog4NetAdapter()
                                            .AddMessageQueue()
                                            .AddPrintView()
                                            .AddScheduler()
                                            .BuildProvider(_configuration);

            return configureServices;
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            app.UseInfinniMiddlewares(resolver);
        }
    }
}