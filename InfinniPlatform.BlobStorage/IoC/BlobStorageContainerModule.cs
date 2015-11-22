﻿using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.BlobStorage.IoC
{
    internal sealed class BlobStorageContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<FileSystemBlobStorageFactory>()
                   .As<IBlobStorageFactory>()
                   .SingleInstance();
        }
    }
}