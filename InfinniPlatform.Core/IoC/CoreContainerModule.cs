﻿using InfinniPlatform.Core.Compression;
using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.RestApi.Public;
using InfinniPlatform.Core.RestQuery.RestQueryBuilders;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Settings;
using InfinniPlatform.Sdk.Global;
using InfinniPlatform.Sdk.IoC;

using PrintViewApi = InfinniPlatform.Core.RestApi.Public.PrintViewApi;
using RegisterApi = InfinniPlatform.Core.RestApi.Public.RegisterApi;

namespace InfinniPlatform.Core.IoC
{
    internal class CoreContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(AppConfiguration.Instance)
                   .As<IAppConfiguration>();

            builder.RegisterType<ScriptContext>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<GZipDataCompressor>()
                   .As<IDataCompressor>()
                   .SingleInstance();

            builder.RegisterType<Log4NetLog>()
                   .As<ILog>()
                   .SingleInstance();

            builder.RegisterType<PerformanceLog>()
                   .As<IPerformanceLog>()
                   .SingleInstance();

            builder.RegisterType<LogComponent>()
                   .As<ILogComponent>()
                   .SingleInstance();

            builder.RegisterType<ProfilerComponent>()
                   .As<IProfilerComponent>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<BinaryManager>()
                   .AsSelf()
                   .SingleInstance();

            // SaaS

            builder.RegisterType<TenantProvider>()
                   .As<ITenantProvider>()
                   .SingleInstance();

            // Transaction

            builder.RegisterType<DocumentTransactionScopeProvider>()
                   .As<IDocumentTransactionScopeProvider>()
                   .SingleInstance();

            // Log4Net
            builder.OnCreateInstance(new Log4NetContainerParameterResolver());
            builder.OnActivateInstance(new Log4NetContainerInstanceActivator());

            // DocumentApi

            builder.RegisterType<DocumentApi>()
                   .AsSelf()
                   .As<IDocumentApi>()
                   .SingleInstance();

            // PrintViewApi

            builder.RegisterType<PrintViewApi>()
                   .As<IPrintViewApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RestApi.DataApi.PrintViewApi>()
                   .AsSelf()
                   .SingleInstance();

            // RegisterApi

            builder.RegisterType<RegisterApi>()
                   .As<IRegisterApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RestApi.DataApi.RegisterApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RegistryComponent>()
                   .As<IRegistryComponent>()
                   .SingleInstance();

            // FileApi

            builder.RegisterType<FileApi>()
                   .As<IFileApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<UploadApi>()
                   .AsSelf()
                   .SingleInstance();

            // AuthApi

            builder.RegisterType<AuthApi>()
                   .As<IAuthApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RestApi.Auth.AuthApi>()
                   .AsSelf()
                   .SingleInstance();

            // CustomServiceApi

            builder.RegisterType<CustomServiceApi>()
                   .As<ICustomServiceApi>()
                   .AsSelf()
                   .SingleInstance();

            // ХЗ

            builder.RegisterType<InprocessDocumentComponent>()
                   .AsSelf()
                   .SingleInstance();

            // Fury

            builder.RegisterType<RestQueryApi>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<LocalQueryBuilder>()
                   .AsSelf()
                   .InstancePerDependency();
        }
    }
}