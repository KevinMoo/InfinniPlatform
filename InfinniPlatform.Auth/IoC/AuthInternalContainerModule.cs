﻿using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.Auth.Identity.UserStore;
using InfinniPlatform.Auth.Middlewares;
using InfinniPlatform.Auth.Services;
using InfinniPlatform.DocumentStorage;
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
        public AuthInternalContainerModule(AuthInternalOptions options)
        {
            _options = options;
        }

        private readonly AuthInternalOptions _options;

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            // AspNet.Identity

            builder.RegisterFactory(CreateUserStore)
                   .As<UserStore<AppUser>>()
                   .As<IUserStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterFactory(CreateRoleStore)
                   .As<RoleStore<AppUserRole>>()
                   .As<IRoleStore<AppUserRole>>()
                   .SingleInstance();
            
            builder.RegisterFactory(CreateUserManager)
                   .As<UserManager<AppUser>>()
                   .ExternallyOwned();

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
            // Хранилище учетных записей пользователей для AspNet.Identity
            var identityUserStore = resolver.Resolve<UserStore<AppUser>>();

            // Провайдер настроек AspNet.Identity
            var optionsAccessor = new OptionsWrapper<IdentityOptions>(new IdentityOptions());

            // Сервис хэширования паролей
            var identityPasswordHasher = new DefaultAppUserPasswordHasher();

            // Валидаторы данных о пользователях
            var userValidators = new List<IUserValidator<AppUser>> {new IdentityApplicationUserValidator(identityUserStore)};

            // Валидатор паролей пользователей
            var passwordValidators = Enumerable.Empty<IPasswordValidator<AppUser>>();

            // Нормализатор
            var keyNormalizer = new UpperInvariantLookupNormalizer();

            // Сервис обработки ошибок AspNet.Identity
            var identityErrorDescriber = new IdentityErrorDescriber();

            // Провайдер зарегистрированных в IoC сервисов
            var serviceProvider = resolver.Resolve<System.IServiceProvider>();

            // Логгер
            var logger = resolver.Resolve<ILogger<UserManager<AppUser>>>();

            var userManager = new UserManager<AppUser>(identityUserStore,
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

        private static UserStore<AppUser> CreateUserStore(IContainerResolver resolver)
        {
            var userDocumentStorage = resolver.Resolve<ISystemDocumentStorage<AppUser>>();
            var userCache = resolver.Resolve<UserCache<AppUser>>();

            return new UserStore<AppUser>(userDocumentStorage, userCache);
        }

        private static RoleStore<AppUserRole> CreateRoleStore(IContainerResolver resolver)
        {
            var documentStorage = resolver.Resolve<IDocumentStorage<AppUserRole>>();

            return new RoleStore<AppUserRole>(documentStorage);
        }
    }
}