﻿using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.InfinniNode
{
    public class UninstallAppTask : IAgentTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public UninstallAppTask(InfinniNodeAdapter infinniNodeAdapter)
        {
            _infinniNodeAdapter = infinniNodeAdapter;
        }

        private readonly InfinniNodeAdapter _infinniNodeAdapter;

        public HttpMethod HttpMethod => HttpMethod.Post;

        public string CommandName => "uninstall";

        public async Task<object> Run(IHttpRequest request)
        {
            var command = CommandName.AppendArg("i", (string)request.Form.AppName)
                                     .AppendArg("v", (string)request.Form.Version)
                                     .AppendArg("n", (string)request.Form.Instance);

            var result = await _infinniNodeAdapter.ExecuteCommand(command, ProcessTimeout);

            var serviceResult = new ServiceResult<TaskStatus> { Success = true, Result = result };

            return serviceResult;
        }
    }
}