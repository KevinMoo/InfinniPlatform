﻿using System;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    ///     Интерфейс для настройки свойств очереди сообщений.
    /// </summary>
    public interface IQueueConfig
    {
        /// <summary>
        ///     Сохранять сообщения на диске.
        /// </summary>
        IQueueConfig Durable();

        /// <summary>
        ///     Уникальный идентификатор обработчика очереди сообщений.
        /// </summary>
        IQueueConfig ConsumerId(string value);

        /// <summary>
        ///     Метод обработки ошибок обработчика очереди сообщений.
        /// </summary>
        IQueueConfig ConsumerError(Action<Exception> value);

        /// <summary>
        ///     Максимальный размер одновременно обрабатываемых сообщений.
        /// </summary>
        /// <remarks>
        ///     Значение 0 соответствует неограниченному размеру.
        /// </remarks>
        IQueueConfig PrefetchSize(int value);

        /// <summary>
        ///     Максимальное количество одновременно обрабатываемых сообщений.
        /// </summary>
        /// <remarks>
        ///     Значение 0 соответствует неограниченному количеству.
        /// </remarks>
        IQueueConfig PrefetchCount(int value);

        /// <summary>
        ///     Количество рабочих потоков для обработки очереди сообщений.
        /// </summary>
        /// <remarks>
        ///     Значение не должно быть меньше 1.
        /// </remarks>
        IQueueConfig WorkerThreadCount(int value);

        /// <summary>
        ///     Метод обработки ошибок рабочего потока очереди сообщений.
        /// </summary>
        IQueueConfig WorkerThreadError(Action<Exception> value);

        /// <summary>
        ///     Минимальное время прослушивания очереди, которое не считается сетевым сбоем.
        /// </summary>
        /// <remarks>
        ///     Время измеряется в миллисекундах.
        /// </remarks>
        IQueueConfig MinListenTime(int value);

        /// <summary>
        ///     Политика подтверждения окончания обработки сообщения.
        /// </summary>
        /// <remarks>
        ///     Подтверждение окончания обработки сообщения сигнализирует серверу очереди сообщений о том, что сообщение обработано
        ///     и может быть
        ///     удалено из очереди. Если сообщение было прочитано, но подтверждения об окончании не поступило, оно остается на
        ///     сервере до тех пор,
        ///     пока не поступит сигнал подтверждения. Это может обеспечить определенную устойчивость к сбоям, которые могут
        ///     возникнуть на стороне
        ///     потребителя. В свою очередь отправитель сообщения может получить уведомление о том, что оно было получено и
        ///     обработано.
        /// </remarks>
        IQueueConfig AcknowledgePolicy(IAcknowledgePolicy value);

        /// <summary>
        ///     Политика подтверждения отказа от обработки сообщения.
        /// </summary>
        /// <remarks>
        ///     Отказ от обработки сообщения может использоваться в случае, когда потребитель не готов по каким-либо причинам
        ///     обработать данное
        ///     сообщение. В этом случае сервер очереди сообщений попытается отправить это сообщение другому потребителю. И так до
        ///     тех пор, пока
        ///     сообщение не будет обработано и серверу не поступит сигнал подтверждения. Однако это абсолютно не гарантирует того,
        ///     что потребитель,
        ///     который когда-либо отказался от обработки данного сообщения, никогда не получит его снова. Механизм отказа от
        ///     обработки сообщения
        ///     ни в коем случае не следует использовать для отсеивания ненужных сообщений. Например, отказ может произойти по
        ///     причине слишком
        ///     большой загруженности узла, на котором осуществляется обработка.
        /// </remarks>
        IQueueConfig RejectPolicy(IRejectPolicy value);
    }
}