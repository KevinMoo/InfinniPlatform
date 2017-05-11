﻿using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Auth.Identity.DocumentStorage;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.Auth.Middlewares;
using InfinniPlatform.Auth.Services;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Http;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.IoC
{
    public class AuthInternalContainerModule : IContainerModule
    {
        private readonly AuthInternalOptions _options;

        public AuthInternalContainerModule(AuthInternalOptions options)
        {
            _options = options;
        }

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            // User storage

            builder.RegisterFactory(_options.UserStoreFactory.GetUserStore<AppUser>)
                   .As<IUserStore<AppUser>>()
                   .SingleInstance();

            // Role storage

            builder.RegisterType<RoleStore<AppUserRole>>()
                   .As<IRoleStore<AppUserRole>>()
                   .SingleInstance();

            // User manager

            builder.RegisterFactory(CreateUserManager)
                   .As<UserManager<AppUser>>()
                   .SingleInstance();

            // Middlewares

            builder.RegisterType<AuthInternalHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            builder.RegisterType<AuthCookieHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // Services

            builder.RegisterType<AuthInternalHttpService>()
                   .As<IHttpService>()
                   .SingleInstance();

            builder.RegisterType<UserEventHandlerInvoker>()
                   .AsSelf()
                   .SingleInstance();

            // UserStorage

            builder.RegisterType<UserCache<AppUser>>()
                   .As<IUserCacheObserver>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AppUserStoreCacheConsumer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AuthInternalConsumerSource>()
                   .As<IConsumerSource>()
                   .SingleInstance();

            builder.RegisterType<AuthInternalDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();
        }


        private static UserManager<AppUser> CreateUserManager(IContainerResolver resolver)
        {
            // Хранилище пользователей
            var appUserStore = resolver.Resolve<IUserStore<AppUser>>();

            // Провайдер настроек AspNet.Identity
            var optionsAccessor = new OptionsWrapper<IdentityOptions>(new IdentityOptions());

            // Сервис хэширования паролей
            var identityPasswordHasher = new DefaultAppUserPasswordHasher();

            // Валидаторы данных о пользователях
            var userValidators = new List<IUserValidator<AppUser>> { new AppUserValidator(appUserStore) };

            // Валидатор паролей пользователей
            var passwordValidators = Enumerable.Empty<IPasswordValidator<AppUser>>();

            // Нормализатор
            var keyNormalizer = new UpperInvariantLookupNormalizer();
            var normalize = keyNormalizer.Normalize("sdfsdf");
            // Сервис обработки ошибок AspNet.Identity
            var identityErrorDescriber = new IdentityErrorDescriber();

            // Провайдер зарегистрированных в IoC сервисов
            var serviceProvider = resolver.Resolve<IServiceProvider>();

            // Логгер
            var logger = resolver.Resolve<ILogger<UserManager<AppUser>>>();

            var userManager = new UserManager<AppUser>(appUserStore,
                                                       optionsAccessor,
                                                       identityPasswordHasher,
                                                       userValidators,
                                                       passwordValidators,
                                                       keyNormalizer,
                                                       identityErrorDescriber,
                                                       serviceProvider,
                                                       logger);

            return userManager;
        }
    }
}