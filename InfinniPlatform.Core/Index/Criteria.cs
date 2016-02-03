﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.Index
{
    /// <summary>
    ///     Критерий отбора по свойству объекта
    /// </summary>
    public class Criteria
    {
        /// <summary>
        ///     Наименование свойства
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        ///     Значение критерия фильтрации
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///     Тип применяемой фильтрации
        /// </summary>
        public CriteriaType CriteriaType { get; set; }
    }


    public static class ElasticSearchQueryBuilderExtensions
    {
        private static readonly Regex ElasticCriteriaRegex = new Regex(@"^[a-zA-Z\.]+$");

        public static IEnumerable<Criteria> GetElasticCriteria(this IEnumerable<Criteria> criteria)
        {
            return criteria.Where(cr => ElasticCriteriaRegex.IsMatch(cr.Property)).ToList();
        }
    }
}