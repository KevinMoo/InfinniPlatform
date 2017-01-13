﻿using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfinniPlatform.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.Core.Http.Hosting
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                    .AddJsonFile("AppCommon.json")
                    .SetBasePath(currentDirectory);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }
        public IContainer ApplicationContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            //builder.RegisterModule(new AutofacContainerModule());

            builder.Populate(services);

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app)
        {
            var config = Configuration;
            var appConfig = new AppEnvironment();
            config.Bind(appConfig);
            var appEnvironment = Configuration.Get<AppEnvironment>();

            //TODO Register Nancy bootstrapper.
            //app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = new HttpServiceNancyBootstrapper()));
        }
    }
}