﻿using System.IO;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
    public sealed class UploadHandler : IWebRoutingHandler
    {
        private readonly IGlobalContext _globalContext;

        public UploadHandler(IGlobalContext globalContext)
        {
            _globalContext = globalContext;
        }

        public IConfigRequestProvider ConfigRequestProvider { get; set; }

        public dynamic UploadFile(dynamic linkedData, Stream uploadStream)
        {
            var config = ConfigRequestProvider.GetConfiguration();
            var metadata = ConfigRequestProvider.GetMetadataIdentifier();
            var version = ConfigRequestProvider.GetVersion();

            var target = new UploadContext
            {
                IsValid = true,
                LinkedData = linkedData,
                Configuration = config,
                Metadata = metadata,
                FileContent = uploadStream,
                Context = _globalContext,
                Version = version
            };

            var metadataConfig =
                _globalContext.GetComponent<IMetadataConfigurationProvider>(ConfigRequestProvider.GetVersion())
                    .GetMetadataConfiguration(version, ConfigRequestProvider.GetConfiguration());

            metadataConfig.MoveWorkflow(metadata, metadataConfig.GetExtensionPointValue(ConfigRequestProvider, "Upload"),
                target);

            if (target.IsValid)
            {
                dynamic item = new DynamicWrapper();
                item.IsValid = true;
                item.ValidationMessage = Resources.DocumentContentSuccessfullyUploaded;
                item.Result = target.Result;
                return AggregateExtensions.PrepareResultAggregate(item);
            }
            return AggregateExtensions.PrepareInvalidResultAggregate(target);
        }
    }
}