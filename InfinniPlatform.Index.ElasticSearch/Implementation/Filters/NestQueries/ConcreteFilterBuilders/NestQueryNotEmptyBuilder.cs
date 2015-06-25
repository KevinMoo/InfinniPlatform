﻿using InfinniPlatform.Api.Index.SearchOptions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestQueries.ConcreteFilterBuilders
{
    internal sealed class NestQueryNotEmptyBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestQuery(Nest.Query<dynamic>.Wildcard(field, "?*"));
        }
    }
}