﻿using System;
using System.IO;

using Infinni.Agent.Helpers;
using Infinni.Agent.Settings;

namespace Infinni.Agent.Providers
{
    public class ConfigurationFileProvider : IConfigurationFileProvider
    {
        private const string AppsDirectoryName = "install";

        public ConfigurationFileProvider(AgentSettings settings)
        {
            _settings = settings;
        }

        private readonly AgentSettings _settings;

        public Func<Stream> Get(string appFullName, string fileName)
        {
            var filePath = Path.Combine(_settings.NodeDirectory, AppsDirectoryName, appFullName, fileName);

            return StreamHelper.TryGetStream(filePath);
        }

        public void Set(string appFullName, string fileName, string content)
        {
            var filePath = Path.Combine(_settings.NodeDirectory, AppsDirectoryName, appFullName, fileName);

            File.WriteAllText(filePath, content);
        }
    }
}