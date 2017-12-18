﻿using InfinniPlatform.BlobStorage;
using InfinniPlatform.BlobStorage.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extensions for file system blob storage services registration.
    /// </summary>
    public static class FileSystemBlobStorageExtensions
    {
        /// <summary>
        /// Register file system blob storage services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddFileSystemBlobStorage(this IServiceCollection services)
        {
            var options = FileSystemBlobStorageOptions.Default;

            return AddFileSystemBlobStorage(services, options);
        }

        /// <summary>
        /// Register file system blob storage services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddFileSystemBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(FileSystemBlobStorageOptions.SectionName).Get<FileSystemBlobStorageOptions>();

            return AddFileSystemBlobStorage(services, options);
        }

        /// <summary>
        /// Register file system blob storage services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="options">File system blob storage options from configuration.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddFileSystemBlobStorage(this IServiceCollection services, FileSystemBlobStorageOptions options)
        {
            return services.AddSingleton(provider => new FileSystemBlobStorageContainerModule(options ?? FileSystemBlobStorageOptions.Default));
        }
    }
}