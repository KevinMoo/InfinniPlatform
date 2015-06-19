﻿using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Index;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    ///   Провайдер для поиска данных по всем индексам и типам, существующим в базе
    /// </summary>
    public sealed class ElasticSearchProviderAllIndexes : IAllIndexesOperationProvider
    {
        private readonly string _routing;
        private readonly ElasticConnection _elasticConnection;

        public ElasticSearchProviderAllIndexes(string routing)
        {
            _routing = routing;
            _elasticConnection = new ElasticConnection();
        }

        /// <summary>
        ///   Получить объект по идентификатору
        /// </summary>
        /// <param name="key">Идентификатор индексируемого объекта</param>
        /// <returns>Индексируемый объект</returns>
        public dynamic GetItem(string key)
        {
            //для объектов типа IndexObject мы не осуществляем
            //поиск по их идентификатору Id. Дело в том, что
            //IndexObject является только служебной оберткой для индексируемого объекта,
            //который находится в свойстве Values. Таким образом, для поиска по идентификатору
            //мы осуществляем поиск по вложенному свойству Values.Id

            var response = _elasticConnection.Client.Search<dynamic>(
                q => q
                    .AllIndices()
                    .AllTypes()
                    .Routing(_routing)
                    .Query(f => f.Term(
                        ElasticConstants.IndexObjectPath + ElasticConstants.IndexObjectIdentifierField,
                        key.ToLowerInvariant())
                    )
                );


            
            dynamic indexObject =
                response.Documents.FirstOrDefault(
                     d => d.Values.Status == null ||
                     (d.Values.Status.ToString() != "Deleted" &&
                     d.Values.Status.ToString() != "Invalid"));

            if (indexObject != null)
            {
                var index = response.Hits.First().Index;
                var type = response.Hits.First().Type;

                var typeName = type.Substring(0,type.LastIndexOf(IndexTypeMapper.MappingTypeVersionPattern,
                    StringComparison.Ordinal));

                dynamic result = DynamicWrapperExtensions.ToDynamic(indexObject.Values);

                result.__ConfigId = index;
                result.__DocumentId = typeName;
                return result;
            }
            return null;
        }
    }
}
