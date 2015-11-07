﻿using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными бизнес-процессов.
	/// </summary>
	internal sealed class ProcessMetadataService : BaseMetadataService
	{
		public ProcessMetadataService(string configId, string documentId)
		{
			ConfigId = configId;
			_documentId = documentId;
		}

		private readonly string _documentId;

		public string ConfigId { get; }

		public override object CreateItem()
		{
			dynamic process = new DynamicWrapper();

			process.Id = Guid.NewGuid().ToString();
			process.Name = string.Empty;
			process.Caption = string.Empty;
			process.Description = string.Empty;

			return process;
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			dynamic configuration = PackageMetadataLoader.GetConfiguration(ConfigId);
			if (configuration.Documents[_documentId].Processes.ContainsKey(item.Name))
			{
				dynamic oldProcess = configuration.Documents[_documentId].Processes[item.Name];
				filePath = oldProcess.FilePath;
			}
			else
			{
				filePath = Path.Combine(Path.GetDirectoryName(configuration.FilePath),
										"Documents",
										_documentId,
										"Processes",
										string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic process = PackageMetadataLoader.GetProcess(ConfigId, _documentId, itemId);

			File.Delete(process.FilePath);

			PackageMetadataLoader.UpdateCache();
		}

		public override object GetItem(string itemId)
		{
			return PackageMetadataLoader.GetProcess(ConfigId, _documentId, itemId);
		}

		public override IEnumerable<object> GetItems()
		{
			return PackageMetadataLoader.GetProcesses(ConfigId, _documentId);
		}
	}
}