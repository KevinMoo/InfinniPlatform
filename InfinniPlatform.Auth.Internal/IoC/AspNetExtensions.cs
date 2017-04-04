﻿using System;
using InfinniPlatform.Auth.Internal.Identity.MongoDb;
using InfinniPlatform.Auth.Internal.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы внутреннего провайдера аутентификации.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddAuth(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddIdentity<IdentityUser, IdentityRole>();
            serviceCollection.AddSingleton(provider => new AuthInternalContainerModule());

            return serviceCollection;
        }

        public static void UseExternalAuth(this IApplicationBuilder app, Action action)
        {
            action.Invoke();
        }
    }
}