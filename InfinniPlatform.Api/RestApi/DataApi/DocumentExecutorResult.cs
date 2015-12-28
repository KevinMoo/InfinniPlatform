﻿namespace InfinniPlatform.Api.RestApi.DataApi
{
    public sealed class DocumentExecutorResult
    {
        public object Id { get; set; }

        public bool IsValid { get; set; }

        public object ValidationMessage { get; set; }

        public object IsInternalServerError { get; set; }
    }
}