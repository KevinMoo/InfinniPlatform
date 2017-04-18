﻿using System;
using System.Threading.Tasks;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;

namespace InfinniPlatform.Cache.TwoLayer
{
    /// <summary>
    /// Обработчик сообщений синхронизации кэша.
    /// </summary>
    [QueueName(nameof(TwoLayerCacheImpl))]
    public class TwoLayerCacheConsumer : BroadcastConsumerBase<string>
    {
        public TwoLayerCacheConsumer(ICacheSynchronizer cache)
        {
            _cache = cache;
        }

        private readonly ICacheSynchronizer _cache;

        protected override async Task Consume(Message<string> message)
        {
            await _cache.ProcessMessage(message);
        }

        protected override Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(false);
        }
    }
}