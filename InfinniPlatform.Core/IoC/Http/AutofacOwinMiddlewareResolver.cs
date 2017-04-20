﻿using System;

using InfinniPlatform.Http.Middlewares;

namespace InfinniPlatform.IoC.Http
{
    internal sealed class AutofacOwinMiddlewareResolver : IOwinMiddlewareTypeResolver
    {
        public Type ResolveType<TOwinMiddleware>() where TOwinMiddleware : OwinMiddleware
        {
            return typeof(AutofacWrapperOwinMiddleware<TOwinMiddleware>);
        }
    }
}