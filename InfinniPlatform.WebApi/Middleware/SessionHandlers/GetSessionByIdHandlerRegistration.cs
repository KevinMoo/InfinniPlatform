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
    public sealed class GetSessionByIdHandlerRegistration : HandlerRegistration
    {
        public GetSessionByIdHandlerRegistration() : base(new RouteFormatterSession(), new RequestPathConstructor(), Owin.Middleware.Priority.Standard, "GET")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context,new PathString(PathConstructor.GetVersionPath() + "/_sessionId_")).Create(Priority.Standard);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            return new ValueRequestHandlerResult(new SessionApi().GetSession(routeDictionary["version"], routeDictionary["sessionId"]));
        }
    }
}
