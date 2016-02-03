﻿using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Core.Index
{
    public static class SearchModelExtensions
    {
        public static SearchModel ExtractSearchModel(this IEnumerable<FilterCriteria> filterCriteria,
                                                     INestFilterBuilder filterFactory)
        {
            var searchModel = new SearchModel();
            
            if (filterCriteria == null)
                return searchModel;

            var filters = filterCriteria.Select(x => filterFactory.Get(x.Property, x.Value, x.CriteriaType));

            searchModel.AddFilters(filters);

            return searchModel;
        }
    }
}