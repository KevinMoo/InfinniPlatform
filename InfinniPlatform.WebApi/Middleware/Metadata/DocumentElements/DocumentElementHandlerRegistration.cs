﻿using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.Metadata.DocumentElements
{
    public abstract class DocumentElementHandlerRegistration : HandlerRegistration
    {
        protected DocumentElementHandlerRegistration(string method)
            : base(new RouteFormatterMetadataDocumentElement(), new RequestPathConstructor(), Priority.Concrete, method)
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            if (Method == "POST" || Method == "PUT")
            {
                return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/metadata/_versionMetadata_/_configuration_/_document_/_metadataType_")).Create(Priority);
            }

            var routeDictionary = RouteFormatter.GetRouteDictionary(context);
            if (routeDictionary.ContainsKey("instanceId") &&
                routeDictionary["instanceId"].ToLowerInvariant() == "unknown" && Method == "GET")
            {
                return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/metadata/_versionMetadata_/_configuration_/_document_/_metadataType_/list")).Create(Priority);
            }

            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/metadata/_versionMetadata_/_configuration_/_document_/_metadataType_/_instanceId_/")).Create(Priority);
        }
    }
}
