﻿using System.Collections.Generic;

using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.Index
{
    /// <summary>
    ///     Провайдер для выполнения агрегаций
    /// </summary>
    public interface IAggregationProvider
    {
        /// <summary>
        ///     Выполнение агрегирующего запроса
        /// </summary>
        /// <param name="dimensions">
        ///     Срезы OLAP куба. Метод позволяет выполнять различные типы срезов (например, Term, Range,
        ///     DateRange)
        /// </param>
        /// <param name="measureTypes">
        ///     Типы измерений. Количество типов измерений должено соответствовать указанному количеству
        ///     measureFieldNames
        /// </param>
        /// <param name="measureFieldNames">Имена свойств, по которым необходимо произвести вычисление</param>
        /// <param name="filters">Фильтр для данных</param>
        /// <returns>Результат выполнения агрегации</returns>
        IEnumerable<AggregationResult> ExecuteAggregation(dynamic[] dimensions, AggregationType[] measureTypes, string[] measureFieldNames, SearchModel filters = null);
          

        /// <summary>
        ///     Выполнение агрегирующего запроса
        /// </summary>
        /// <param name="filters">Фильтр для данных</param>
        /// <param name="termFields">Имена полей, по которым будут вычислены Term срезы OLAP куба</param>
        /// <param name="measureType">Тип измерения</param>
        /// <param name="measureFieldName">Имя поля, по которому необходимо произвести вычисление</param>
        /// <returns>Результат выполнения агрегации</returns>
        IEnumerable<AggregationResult> ExecuteTermAggregation(
            IEnumerable<string> termFields,
            AggregationType measureType,
            string measureFieldName,
            SearchModel filters = null);
    }
}