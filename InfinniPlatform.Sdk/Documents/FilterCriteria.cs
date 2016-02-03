﻿using System;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.Documents
{
    [Serializable]
    public sealed class FilterCriteria
    {
        public FilterCriteria(string property, object value, CriteriaType criteriaType)
        {
            Property = property;
            Value = value;
            CriteriaType = criteriaType;
        }

        public string Property { get; set; }

        public object Value { get; set; }

        public CriteriaType CriteriaType { get; set; }

        public string ToJsonString()
        {
            var criteriaFilter = new FilterCriteria(Property, Value, CriteriaType);
            var serializeObject = JsonConvert.SerializeObject(criteriaFilter);

            return serializeObject;
        }
    }
}