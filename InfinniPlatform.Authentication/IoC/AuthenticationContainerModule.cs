﻿using InfinniPlatform.Authentication.Hosting;
using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Authentication.Modules;
using InfinniPlatform.Authentication.Security;
using InfinniPlatform.Authentication.UserStorage;
using InfinniPlatform.Core.Security;
using InfinniPlatform.DocumentStorage.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Settings;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.IoC
{
    internal sealed class AuthenticationContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Менеджер работы с учетными записями пользователей для AspNet.Identity
            builder.RegisterFactory(CreateUserManager)
                   .As<UserManager<IdentityApplicationUser>>()
                   .ExternallyOwned();

            // Сервис хэширования паролей пользователей на уровне приложения
            builder.RegisterType<DefaultApplicationUserPasswordHasher>()
                   .As<IApplicationUserPasswordHasher>()
                   .SingleInstance();

            // Менеджер работы с учетными записями пользователей на уровне приложения
            builder.RegisterType<IdentityApplicationUserManager>()
                   .As<IApplicationUserManager>()
                   .SingleInstance();

            // Идентификационные данных текущего пользователя
            builder.RegisterType<UserIdentityProvider>()
                   .As<IUserIdentityProvider>()
                   .SingleInstance();

            // Модули аутентификации

            builder.RegisterType<CookieAuthOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ExternalAuthAdfsOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ExternalAuthFacebookOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ExternalAuthGoogleOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ExternalAuthVkOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<InternalAuthOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<UserStorageSettings>(UserStorageSettings.SectionName))
                   .As<UserStorageSettings>()
                   .SingleInstance();

            builder.RegisterType<ApplicationUserStoreCache>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ApplicationUserStore>()
                   .As<IApplicationUserStore>()
                   .SingleInstance();

            // Сервисы аутентификации

            builder.RegisterHttpServices(GetType().Assembly);

            // Документы

            builder.RegisterType<AuthenticationDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();
        }

        private static UserManager<IdentityApplicationUser> CreateUserManager(IContainerResolver resolver)
        {
            var appUserStore = resolver.Resolve<IApplicationUserStore>();
            var appPasswordHasher = resolver.Resolve<IApplicationUserPasswordHasher>();

            // Хранилище учетных записей пользователей для AspNet.Identity
            var identityUserStore = new IdentityApplicationUserStore(appUserStore);

            // Сервис проверки учетных записей пользователей для AspNet.Identity
            var identityUserValidator = new IdentityApplicationUserValidator(identityUserStore);

            // Сервис хэширования паролей пользователей для AspNet.Identity
            var identityPasswordHasher = new IdentityApplicationUserPasswordHasher(appPasswordHasher);

            var userManager = new UserManager<IdentityApplicationUser>(identityUserStore)
                              {
                                  UserValidator = identityUserValidator,
                                  PasswordHasher = identityPasswordHasher
                              };

            return userManager;
        }
    }
}