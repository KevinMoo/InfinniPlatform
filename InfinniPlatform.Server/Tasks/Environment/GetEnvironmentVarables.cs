﻿using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Server.Agent;

namespace InfinniPlatform.Server.Tasks.Environment
{
    public class GetEnvironmentVarables : IServerTask
    {
        public GetEnvironmentVarables(IAgentHttpClient agentHttpClient)
        {
            _agentHttpClient = agentHttpClient;
        }

        private readonly IAgentHttpClient _agentHttpClient;

        public string CommandName => "variables";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public async Task<object> Run(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var environmentVariables = await _agentHttpClient.Get<ServiceResult<object>>(CommandName, address, port);

            return environmentVariables;
        }
    }
}