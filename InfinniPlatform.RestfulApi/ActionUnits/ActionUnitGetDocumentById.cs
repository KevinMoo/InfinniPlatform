﻿using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    /// <summary>
    ///     Модуль для получения документа по указанному идентификатору
    ///     Особенность: при выполнении запроса по идентификатору НЕ выполняется проверка прав (неизвестен индекс и тип получаемого документа)
    /// </summary>
    public sealed class ActionUnitGetDocumentById
    {
        public void Action(IApplyContext target)
        {
            var executor =
                new DocumentExecutor(target.Context.GetComponent<IConfigurationMediatorComponent>(target.Version),
                                     target.Context.GetComponent<IMetadataComponent>(target.Version),
                                     target.Context.GetComponent<InprocessDocumentComponent>(target.Version),
                                     target.Context.GetComponent<IProfilerComponent>(target.Version));

            if (string.IsNullOrEmpty(target.Item.ConfigId) || string.IsNullOrEmpty(target.Item.DocumentId))
            {
                target.Result = executor.GetBaseDocument(target.UserName, target.Item.Id);
            }
            else
            {
                target.Result = executor.GetCompleteDocument(target.Version, target.Item.ConfigId,
                                                             target.Item.DocumentId,
                                                             target.UserName, target.Item.Id);
            }
        }
    }
}