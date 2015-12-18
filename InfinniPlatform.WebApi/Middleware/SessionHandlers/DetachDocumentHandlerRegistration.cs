﻿using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.SessionHandlers
{
    public sealed class DetachDocumentHandlerRegistration : HandlerRegistration
    {
        public DetachDocumentHandlerRegistration(SessionApi sessionApi) : base(new RouteFormatterSession(), new RequestPathConstructor(), Priority.Standard, "DELETE")
        {
            _sessionApi = sessionApi;
        }

        private readonly SessionApi _sessionApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/_sessionId_/_attachmentId_")).Create(Priority.Standard);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            return new ValueRequestHandlerResult(_sessionApi.Detach(routeDictionary["sessionId"], routeDictionary["attachmentId"]));
        }
    }
}