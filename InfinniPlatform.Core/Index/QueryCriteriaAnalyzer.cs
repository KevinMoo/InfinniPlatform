﻿using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Schema;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.Index
{
    /// <summary>
    ///     Анализатор критериев запроса данных
    /// </summary>
    public sealed class QueryCriteriaAnalyzer
    {
        private readonly List<SchemaObject> _resolveProperties = new List<SchemaObject>();

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="metadataComponent">Провайдер метаданных сервисной части</param>
        /// <param name="schema">Схема данных документа, к которому выполняется запрос</param>
        public QueryCriteriaAnalyzer(IMetadataComponent metadataComponent, dynamic schema)
        {
            var metadataIterator = new SchemaIterator(new SchemaProvider(metadataComponent))
                                   {
                                       OnObjectProperty = schemaObject =>
                                                          {
                                                              if (schemaObject.IsResolve)
                                                              {
                                                                  _resolveProperties.Add(schemaObject);
                                                              }
                                                          },
                                       OnArrayProperty = schemaObject =>
                                                         {
                                                             if (schemaObject.IsDocumentArray)
                                                             {
                                                                 _resolveProperties.Add(schemaObject);
                                                             }
                                                         }
                                   };
            metadataIterator.ProcessSchema(schema);
        }

        /// <summary>
        ///     Получить список критериев документа, применяемых до выполнения операции Resolve
        /// </summary>
        /// <param name="filter">Список критериев запроса</param>
        /// <returns>Список критериев</returns>
        public IEnumerable<CriteriaFilter> GetBeforeResolveCriteriaList(IEnumerable<CriteriaFilter> filter)
        {
            return filter.Where(criteria => !_resolveProperties.Any(r => criteria.Property.StartsWith(r.GetFullPath())));
        }

        /// <summary>
        ///     Получить список критериев запроса, выполняемых после операции Resolve
        /// </summary>
        /// <param name="filter">Список критериев запроса</param>
        /// <returns>Список критериев</returns>
        public IEnumerable<CriteriaFilter> GetAfterResolveCriteriaList(IEnumerable<CriteriaFilter> filter)
        {
            var filtersArray = filter.ToArray();
            var beforeResolveCriteriaList = GetBeforeResolveCriteriaList(filtersArray);

            return filtersArray.Except(beforeResolveCriteriaList);
        }
    }
}