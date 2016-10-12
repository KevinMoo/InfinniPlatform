﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.Agent
{
    public class AgentConnector : IAgentConnector
    {
        private const string InstallPath = "install";
        private const string UninstallPath = "uninstall";
        private const string InitPath = "init";
        private const string StartPath = "start";
        private const string StopPath = "stop";
        private const string RestartPath = "restart";
        private const string AppsInfoPath = "appsInfo";
        private const string ConfigPath = "config";
        private const string VariablesPath = "variables";
        private const string VariablePath = "variable";

        public AgentConnector(ServerSettings serverSettings, IJsonObjectSerializer serializer)
        {
            _httpClient = new HttpClient();
            _serverSettings = serverSettings;
            _serializer = serializer;
        }

        private readonly HttpClient _httpClient;
        private readonly IJsonObjectSerializer _serializer;
        private readonly ServerSettings _serverSettings;

        public AgentInfo[] GetAgentsInfo()
        {
            return _serverSettings.AgentsInfo;
        }

        public async Task<object> GetAppsInfo(string agentAddress, int agentPort)
        {
            return await ExecuteGetRequest(AppsInfoPath, agentAddress, agentPort);
        }

        public async Task<object> GetVariables(string agentAddress, int agentPort)
        {
            return await ExecuteGetRequest(VariablesPath, agentAddress, agentPort);
        }

        public async Task<object> InstallApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecutePostRequest(InstallPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> UninstallApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecutePostRequest(UninstallPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> InitApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecutePostRequest(InitPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> StartApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecutePostRequest(StartPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> StopApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecutePostRequest(StopPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> RestartApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecutePostRequest(RestartPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> GetAppsInfo(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecuteGetRequest(AppsInfoPath, agentAddress, agentPort);
        }

        public async Task<object> GetConfigurationFile(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecuteGetRequest(ConfigPath, agentAddress, agentPort);
        }

        public async Task<object> SetConfigurationFile(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecutePostRequest(ConfigPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> GetVariable(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            return await ExecuteGetRequest(VariablePath, agentAddress, agentPort);
        }

        private async Task<object> ExecuteGetRequest(string path, string agentAddress, int agentPort, DynamicWrapper queryContent = null)
        {
            var uriString = $"http://{agentAddress}:{agentPort}/agent/{path}{ToQuery(queryContent)}";
            var response = await _httpClient.GetAsync(uriString);
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        private async Task<object> ExecutePostRequest(string path, string agentAddress, int agentPort, DynamicWrapper formContent)
        {
            var uriString = $"http://{agentAddress}:{agentPort}/agent/{path}";

            var convertToString = _serializer.ConvertToString(formContent);

            var requestContent = new StringContent(convertToString, _serializer.Encoding, HttpConstants.JsonContentType);

            var response = await _httpClient.PostAsync(new Uri(uriString), requestContent);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private static string ToQuery(DynamicWrapper queryContent)
        {
            if (queryContent == null)
            {
                return null;
            }

            var query = "?";

            foreach (var pair in queryContent.ToDictionary())
            {
                query += $"{pair.Key}={pair.Value}&";
            }

            return query.TrimEnd('&');
        }
    }
}