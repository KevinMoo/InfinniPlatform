﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticSearchModels;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestQueries;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeSelectors;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
	/// <summary>
	/// Исполнитель поисковых запросов к ElasticSearch
	/// </summary>
	public sealed class IndexQueryExecutor : IIndexQueryExecutor
	{
		public IndexQueryExecutor(IndexToTypeAccordanceSettings settings)
		{
			_elasticConnection = new ElasticConnection();
			_indexNames = settings.Accordances.Select(i => i.IndexName).ToArray();
			_typeNames = settings.Accordances.ToArray();
			_searchInAllIndeces = settings.SearchInAllIndeces;
			_searchInAllTypes = settings.SearchInAllTypes;
			_elasticConnection.ConnectIndex();
		}

		private readonly ElasticConnection _elasticConnection;
		// Имена индексов, использующиеся при поисковых запросах по нескольким индексам
		private readonly IEnumerable<string> _indexNames;
		private readonly bool _searchInAllIndeces;
		private readonly bool _searchInAllTypes;
		private readonly IEnumerable<IndexToTypeAccordance> _typeNames;

		/// <summary>
		/// Найти список объектов в индексе по указанной модели поиска
		/// </summary>
		/// <param name="searchModel">Модель поиска</param>
		/// <returns>Модель результат поиска объектов</returns>
		public SearchViewModel Query(SearchModel searchModel)
		{
			return QueryOverObject(searchModel, (item, index, type) =>
												{
													dynamic result = DynamicWrapperExtensions.ToDynamic(item.Values);
													result.__ConfigId = index;
													result.__DocumentId = type;
													result.TimeStamp = item.TimeStamp;
													return result;
												});
		}

		/// <summary>
		/// Определить количество объектов в индексе по указанной модели поиска
		/// </summary>
		/// <param name="searchModel">Модель поиска</param>
		/// <returns>Количество объектов, удовлетворяющих условиям поиска</returns>
		public long CalculateCountQuery(SearchModel searchModel)
		{
            var tenantId = CachedSecurityComponent.Instance.GetClaim(AuthorizationStorageExtensions.OrganizationClaim, "TestOverlord") ??
                       AuthorizationStorageExtensions.AnonimousUser;
            searchModel.AddFilter(new NestQuery(Query<dynamic>.Term(ElasticConstants.TenantIdField, tenantId)));
			searchModel.AddFilter(new NestQuery(Query<dynamic>.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid)));

			Func<CountDescriptor<dynamic>, CountDescriptor<dynamic>> desc =
				descriptor => new ElasticCountQueryBuilder(descriptor)
					.BuildCountQueryDescriptor(searchModel)
					.BuildSearchForType(_indexNames,
						(_typeNames == null || !_typeNames.Any()) ? null : _typeNames.SelectMany(d => d.TypeNames),
						_searchInAllIndeces, _searchInAllTypes);


			var documentsResponse = _elasticConnection.Client.Count(desc);

			return documentsResponse != null ? documentsResponse.Count : 0;
		}

		/// <summary>
		/// Выполнить запрос с получением объектов индекса без дополнительной обработки
		/// </summary>
		/// <param name="searchModel">Модель поиска</param>
		/// <param name="convert">Делегат для конвертирования результата</param>
		/// <returns>Результаты поиска</returns>
		public SearchViewModel QueryOverObject(SearchModel searchModel, Func<dynamic, string, string, object> convert)
		{
			var userName = string.Empty;

			try
			{
				userName = Thread.CurrentPrincipal.Identity.Name;
			}
			catch (Exception)
			{
				//ignored
			}

			var tenantId = CachedSecurityComponent.Instance.GetClaim(AuthorizationStorageExtensions.OrganizationClaim, userName) ??
						   MultitenancyProvider.GetTenantId(null, _indexNames.FirstOrDefault());
			
            searchModel.AddFilter(new NestFilter(Filter<dynamic>.Query(q => q.Term(ElasticConstants.TenantIdField, tenantId))));
			searchModel.AddFilter(new NestFilter(Filter<dynamic>.Query(q => q.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid))));

			Func<SearchDescriptor<dynamic>, SearchDescriptor<dynamic>> desc =
				descriptor => new ElasticSearchQueryBuilder(descriptor)
					.BuildSearchDescriptor(searchModel)
					.BuildSearchForType(_indexNames,
						(_typeNames == null || !_typeNames.Any()) ? null : _typeNames.SelectMany(d => d.TypeNames), _searchInAllIndeces, _searchInAllTypes);


			var documentsResponse = _elasticConnection.Client.Search(desc);

			var hitsCount = documentsResponse != null && documentsResponse.Hits != null
				? documentsResponse.Hits.Count()
				: 0;

			var documentResponseCount = documentsResponse != null &&
										documentsResponse.Hits != null
				? documentsResponse.Hits.Select(r => convert(r.Source, r.Index, ToDocumentId(r.Type))).ToList()
				: new List<dynamic>();

			return new SearchViewModel(searchModel.FromPage, searchModel.PageSize, hitsCount, documentResponseCount);
		}

		/// <summary>
		/// Метод удаляет окончание _typeschema_ из имени типа
		/// </summary>
		private static string ToDocumentId(string fullTypeName)
		{
			var documentId = fullTypeName;

			var posSchema = fullTypeName.IndexOf(IndexTypeMapper.MappingTypeVersionPattern, StringComparison.Ordinal);
			if (posSchema > -1)
			{
				documentId = fullTypeName.Substring(0, posSchema);
			}

			return documentId;
		}
	}
}