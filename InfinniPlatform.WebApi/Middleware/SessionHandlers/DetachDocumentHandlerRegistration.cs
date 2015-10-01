﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware.SessionHandlers
{
    public sealed class DetachDocumentHandlerRegistration : HandlerRegistration
    {
        public DetachDocumentHandlerRegistration() : base(new RouteFormatterSession(), new RequestPathConstructor(), Priority.Standard, "DELETE")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/_sessionId_/_attachmentId_")).Create(Priority.Standard);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            return new ValueRequestHandlerResult(new SessionApi().Detach(routeDictionary["sessionId"], routeDictionary["attachmentId"]));
        }
    }
}