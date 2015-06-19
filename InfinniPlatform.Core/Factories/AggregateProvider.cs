﻿using System;
using System.Collections.Generic;
using InfinniPlatform.Json.EventBuilders;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.Sdk.Application.Events;

namespace InfinniPlatform.Factories
{
    public class AggregateProvider
    {
        public object CreateAggregate()
        {
            dynamic result = new DynamicWrapper();
            result.Id = Guid.NewGuid().ToString().ToLowerInvariant();
            return result;
        }

        public void ApplyChanges(ref object item, IEnumerable<EventDefinition> events)
        {
            if (item == null)
            {
                throw new ArgumentException("Need to set object to apply changes");
            }
            if (events == null)
            {
                throw new ArgumentException("Need to set event list to apply");
            }

            item = new BackboneBuilderJson().ConstructJsonObject(item, events);
        }
    }
}