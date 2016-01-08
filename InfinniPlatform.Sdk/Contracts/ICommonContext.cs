﻿namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    /// Общий контекст выполнения всех точек расширения
    /// </summary>
    public interface ICommonContext
    {
        /// <summary>
        /// Результат фильтрации событий
        /// </summary>
        dynamic ValidationMessage { get; set; }

        /// <summary>
        /// Признак успешности обработки события фильтрации событий
        /// </summary>
        bool IsValid { get; set; }

        /// <summary>
        /// Признак системной ошибки сервера
        /// </summary>
        bool IsInternalServerError { get; set; }

        /// <summary>
        /// Версия исполняемой точки расширения
        /// </summary>
        /// <summary>
        /// Конфигурация текущего запроса
        /// </summary>
        string Configuration { get; set; }

        /// <summary>
        /// Метаданные текущего запроса
        /// </summary>
        string Metadata { get; set; }

        /// <summary>
        /// Действие текущего запроса
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Авторизованный пользователь системы
        /// </summary>
        string UserName { get; set; }
    }
}