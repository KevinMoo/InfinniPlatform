﻿namespace InfinniPlatform.Sdk.Queues.Integration
{
    /// <summary>
    ///     Менеджер для хранилища подписок интеграционной шины.
    /// </summary>
    public interface IIntegrationBusSubscriptionStorageManager
    {
        /// <summary>
        ///     Создать хранилище.
        /// </summary>
        void CreateStorage();

        /// <summary>
        ///     Удалить хранилище.
        /// </summary>
        void DeleteStorage();
    }
}