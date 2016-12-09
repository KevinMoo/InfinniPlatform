﻿using System.Net.Http;
using System.Threading.Tasks;

using Infinni.Agent.Providers;
using InfinniPlatform.Sdk.Http.Services;

namespace Infinni.Agent.Tasks.Files
{
    public class PerfLogTask : IAgentTask
    {
        public PerfLogTask(ILogFilePovider logFilePovider)
        {
            _logFilePovider = logFilePovider;
        }

        private readonly ILogFilePovider _logFilePovider;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "perfLog";

        public Task<object> Run(IHttpRequest request)
        {
            string appFullName = request.Query.FullName;

            var streamHttpResponse = new StreamHttpResponse(_logFilePovider.GetPerformanceLog(appFullName), "application/text");
            return Task.FromResult<object>(streamHttpResponse);
        }
    }
}