﻿using InfinniPlatform.Api.SearchOptions;

namespace InfinniPlatform.Api.Index.SearchOptions
{
    public class SortOption
    {
        public SortOption(string field, SortOrder order)
        {
            Order = order;
            Field = field;
        }

        public string Field { get; private set; }
        public SortOrder Order { get; private set; }
    }
}