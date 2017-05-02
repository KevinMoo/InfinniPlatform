﻿using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.Management
{
    internal static class RabbitMqDefaults
    {
        public static class Connection
        {
            /// <summary>
            /// Автоматически переподключаться к серверу при потере соединения.
            /// </summary>
            public static bool AutomaticRecoveryEnabled => true;
        }


        public static class Exchange
        {
            /// <summary>
            /// Точка обмена не удаляется при перезапуске сервера.
            /// </summary>
            public static bool Durable => true;

            /// <summary>
            /// Точка обмена не удаляется при отключении всех очередей.
            /// </summary>
            public static bool AutoDelete => false;


            /// <summary>
            /// Типы точек обмена.
            /// </summary>
            public static class Type
            {
                public static string Topic => ExchangeType.Topic;

                public static string Direct => ExchangeType.Direct;

                public static string Fanout => ExchangeType.Fanout;
            }
        }


        public static class Queue
        {
            /// <summary>
            /// Очередь не удаляется при перезапуске сервера.
            /// </summary>
            public static bool Durable => true;

            /// <summary>
            /// Очередь может быть доступна для всех подключений.
            /// </summary>
            public static bool Exclusive => false;

            /// <summary>
            /// Очередь не удаляется после отключения всех потребителей.
            /// </summary>
            public static bool AutoDelete => false;
        }
    }
}