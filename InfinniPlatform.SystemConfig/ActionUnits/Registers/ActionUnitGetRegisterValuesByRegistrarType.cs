﻿using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Registers;
using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Core.SearchOptions.Builders;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.SystemConfig.ActionUnits.Registers
{
    /// <summary>
    /// Получение значений ресурсов по типу документа-регистратора
    /// </summary>
    public sealed class ActionUnitGetRegisterValuesByRegistrarType
    {
        public ActionUnitGetRegisterValuesByRegistrarType(RestQueryApi restQueryApi, IMetadataComponent metadataComponent)
        {
            _restQueryApi = restQueryApi;
            _metadataComponent = metadataComponent;
        }

        private readonly RestQueryApi _restQueryApi;
        private readonly IMetadataComponent _metadataComponent;

        public void Action(IApplyResultContext target)
        {
            var registrarType = target.Item.RegistrarType;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();
            var specifiedDimensions = target.Item.Dimensions;

            var registerObject = _metadataComponent
                      .GetMetadataList(configurationId, registerId, MetadataType.Register)
                      .FirstOrDefault();

            if (registerObject == null)
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = $"Register '{registerId}' not found.";
                return;
            }

            IEnumerable<dynamic> dimensions = specifiedDimensions == null
                ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                : AggregationUtils.BuildDimensionsFromProperties(specifiedDimensions);

            var valueProperties = target.Item.ValueProperties ??
                                  AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.RegistrarTypeProperty).IsEquals(registrarType));

            IEnumerable<dynamic> aggregationResult = _restQueryApi.QueryAggregationRaw(
                "SystemConfig",
                "metadata",
                "aggregate",
                configurationId,
                RegisterConstants.RegisterNamePrefix + registerId,
                filetrBuilder.GetFilter(),
                dimensions,
                AggregationUtils.BuildAggregationType(AggregationType.Sum,
                    valueProperties is List<string>
                        ? valueProperties.Count
                        : valueProperties.Count()),
                valueProperties.ToArray(),
                0,
                10000)
                                                                  .ToDynamicList();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            target.Result = AggregationUtils.ProcessBuckets(dimensions.Select(d => (string)d.FieldName).ToArray(),
                valueProperties.ToArray(), aggregationResult);
        }
    }
}