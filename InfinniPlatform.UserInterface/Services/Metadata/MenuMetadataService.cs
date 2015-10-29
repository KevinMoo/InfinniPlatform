﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными меню.
	/// </summary>
	internal sealed class MenuMetadataService : BaseMetadataService
	{
		public MenuMetadataService(string version, string configId, string server, int port, string route)
			: base(version, server, port, route)
		{
			_configId = configId;
		}

		private readonly string _configId;

		public string ConfigId
		{
			get { return _configId; }
		}

		public override object CreateItem()
		{
			return new DynamicWrapper();
		}

		public override void ReplaceItem(dynamic item)
		{
			string filePath;
			var serializedItem = JsonObjectSerializer.Formated.Serialize(item);

			//TODO Wrapper for PackageMetadataLoader.Configurations
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			if (configuration.Menu.ContainsKey(item.Name))
			{
				dynamic oldMenu = configuration.Documents[item.Name];
				filePath = oldMenu.FilePath;
			}
			else
			{
				string directoryPath = Path.Combine(Path.GetDirectoryName(configuration.FilePath), "Menu", item.Name);
				Directory.CreateDirectory(directoryPath);

				filePath = Path.Combine(directoryPath, string.Concat(item.Name, ".json"));
			}

			File.WriteAllBytes(filePath, serializedItem);

			PackageMetadataLoader.UpdateCache();
		}

		public override void DeleteItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			var menu = configuration.Menu[itemId];

			var menuDirectory = Path.GetDirectoryName(menu.FilePath);

			if (menuDirectory != null)
			{
				Directory.Delete(menuDirectory, true);
				PackageMetadataLoader.UpdateCache();
			}
		}

		public override object GetItem(string itemId)
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			return configuration.Menu[itemId].Content;
		}

		public override IEnumerable<object> GetItems()
		{
			dynamic configuration = PackageMetadataLoader.Configurations[_configId];
			Dictionary<string, dynamic> documents = configuration.Menu;
			return documents.Values.Select(o => o.Content);
		}
	}
}