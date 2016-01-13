﻿using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.Filters;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Versioning
{
    /// <summary>
    /// Провайдер операций с версионными данными без использования истории
    /// </summary>
    public sealed class VersionProvider : IVersionProvider
    {
        public VersionProvider(ICrudOperationProvider elasticSearchProvider,
                               IIndexQueryExecutor indexQueryExecutor)
        {
            _elasticSearchProvider = elasticSearchProvider;
            _indexQueryExecutor = indexQueryExecutor;
        }

        private readonly ICrudOperationProvider _elasticSearchProvider;
        private readonly IIndexQueryExecutor _indexQueryExecutor;

        /// <summary>
        /// Получить актуальные версии объектов, отсортированные по дате вставки в индекс по убыванию
        /// </summary>
        /// <param name="filterCriteria">Фильтр объектов</param>
        /// <param name="pageNumber">Номер страницы данных</param>
        /// <param name="pageSize">Размер страницы данных</param>
        /// <param name="sortingCriteria">Описание правил сортировки</param>
        /// <param name="skipSize"></param>
        /// <returns>Список актуальных версий</returns>
        public dynamic GetDocument(IEnumerable<FilterCriteria> filterCriteria, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sortingCriteria = null, int skipSize = 0)
        {
            var filterFactory = FilterBuilderFactory.GetInstance();
            var searchModel = filterCriteria.ExtractSearchModel(filterFactory);
            searchModel.SetPageSize(pageSize);
            searchModel.SetSkip(skipSize);
            searchModel.SetFromPage(pageNumber);

            if (sortingCriteria != null)
            {
                foreach (var sorting in sortingCriteria)
                {
                    searchModel.AddSort(sorting.PropertyName, sorting.SortingOrder);
                }
            }

            return _indexQueryExecutor.Query(searchModel).Items.ToList();
        }

        /// <summary>
        /// Получить общее количество объектов по заданному фильтру
        /// </summary>
        /// <param name="filterCriteria">Фильтр объектов</param>
        /// <returns>Количество объектов</returns>
        public int GetNumberOfDocuments(IEnumerable<FilterCriteria> filterCriteria)
        {
            var queryFactory = QueryBuilderFactory.GetInstance();
            var searchModel = filterCriteria.ExtractSearchModel(queryFactory);

            // вряд ли документов в одном индексе будет больше чем 2 147 483 647, конвертируем в int
            return Convert.ToInt32(_indexQueryExecutor.CalculateCountQuery(searchModel));
        }

        /// <summary>
        /// Получить версию по уникальному идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор версии</param>
        /// <returns>Версия объекта</returns>
        public dynamic GetDocument(string id)
        {
            return _elasticSearchProvider.GetItem(id);
        }

        /// <summary>
        /// Получить список версий по уникальному идентификатору
        /// </summary>
        /// <param name="ids">Список идентификаторов версий</param>
        /// <returns>Список версий</returns>
        public IEnumerable<dynamic> GetDocuments(IEnumerable<string> ids)
        {
            return _elasticSearchProvider.GetItems(ids);
        }

        /// <summary>
        /// Удалить документ
        /// </summary>
        /// <param name="id">Идентификатор версии</param>
        public void DeleteDocument(string id)
        {
            _elasticSearchProvider.Remove(id);
            _elasticSearchProvider.Refresh();
        }

        /// <summary>
        /// Удалить документы с идентификаторами из списка
        /// </summary>
        /// <param name="ids">Список идентификаторов</param>
        public void DeleteDocuments(IEnumerable<string> ids)
        {
            _elasticSearchProvider.RemoveItems(ids);
            _elasticSearchProvider.Refresh();
        }

        /// <summary>
        /// Записать версию объекта в индекс
        /// </summary>
        /// <param name="version">Обновляемая версия объекта</param>
        public void SetDocument(dynamic version)
        {
            if (version.Id == null)
            {
                version.Id = Guid.NewGuid().ToString();
            }

            _elasticSearchProvider.Set(version);
            _elasticSearchProvider.Refresh();
        }

        /// <summary>
        /// Вставить список версий в индекс
        /// </summary>
        /// <param name="versions">Список версий</param>
        public void SetDocuments(IEnumerable<dynamic> versions)
        {
            _elasticSearchProvider.SetItems(versions);
            _elasticSearchProvider.Refresh();
        }
    }
}