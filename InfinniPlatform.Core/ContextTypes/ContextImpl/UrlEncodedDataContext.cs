﻿using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.ContextTypes.ContextImpl
{
    public sealed class UrlEncodedDataContext : IUrlEncodedDataContext
    {
        public dynamic ValidationMessage { get; set; }
        public bool IsValid { get; set; }
        public bool IsInternalServerError { get; set; }
        public string Configuration { get; set; }
        public string Metadata { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }
        public dynamic Result { get; set; }
        public dynamic FormData { get; set; }
    }
}