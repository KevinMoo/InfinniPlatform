﻿using InfinniPlatform.Api.Index.SearchOptions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestQueries.ConcreteFilterBuilders
{
    internal sealed class NestQueryStartsWithBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestQuery(Nest.Query<dynamic>.Wildcard(field, value + "*"));
        }
    }
}
