﻿using System.Threading.Tasks;

using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Auth.UserStorage
{
    /// <summary>
    /// Интерфейс синхронизации кэша пользователей через очередь сообщений.
    /// </summary>
    internal interface IUserCacheSynchronizer
    {
        /// <summary>
        /// Обрабатывает сообщение из очереди.
        /// </summary>
        /// <param name="message">Сообщение из очереди.</param>
        Task ProcessMessage(Message<string> message);
    }
}