﻿using System;

namespace InfinniPlatform.Core.MessageQueue
{
    /// <summary>
    ///     Интерфейс для объявления очередей, связанных с точкой обмена сообщениями типа "Topic".
    /// </summary>
    /// <remarks>
    ///     Маршрутизация сообщений для точек обмена с типом "Topic" осуществляется от издателя во все очереди, связанные с
    ///     точкой обмена и
    ///     имеющие шаблон маршрутизации, которому удовлетворяет ключ маршрутизации, указанный при отправке сообщения.
    /// </remarks>
    public interface IExchangeTopicBinding
    {
        /// <summary>
        ///     Подписаться на очередь сообщений.
        /// </summary>
        /// <param name="queue">Наименование очереди сообщений.</param>
        /// <param name="consumer">Метод для получения прослушивателя очереди сообщений.</param>
        /// <param name="routingPattern">Шаблон маршрутизации очереди сообщений.</param>
        /// <param name="config">Метод для настройки свойств очереди сообщений.</param>
        /// <remarks>
        ///     При формировании шаблона маршрутизации используются следующие соглашения. Шаблон должен быть пустым, либо состоять
        ///     из слов,
        ///     разделенных точками. Каждое слово может содержать символы из набора [a-zA-Z0-9]. Дополнительно могут быть
        ///     использованы
        ///     специальные символы: * - означает одно слово, # - означает любое количество слов.
        /// </remarks>
        void Subscribe(string queue, Func<IQueueConsumer> consumer, string routingPattern,
            Action<IQueueConfig> config = null);

        /// <summary>
        ///     Удалить подписку на очередь сообщений.
        /// </summary>
        /// <param name="queue">Наименование очереди сообщений.</param>
        /// <remarks>
        ///     При удалении подписки на очередь сообщений, удаляется очередь и все сообщения, находящиеся в ней.
        /// </remarks>
        void Unsubscribe(string queue);
    }
}