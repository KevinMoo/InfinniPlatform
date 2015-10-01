﻿using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Исполнитель скриптов из глобального контекста
    /// </summary>
    public interface IScriptRunnerComponent
    {
        /// <summary>
        ///     Получить исполнителя скриптов для указанного идентификатора метаданных конфигурации
        /// </summary>
        /// <param name="version"></param>
        /// <param name="configurationId">Идентификатор метаданных конфигурации</param>
        /// <returns>Исполнитель скриптов</returns>
        IScriptProcessor GetScriptRunner(string version, string configurationId);
    }
}