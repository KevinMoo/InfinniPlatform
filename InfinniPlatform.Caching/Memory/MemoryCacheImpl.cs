﻿using System;
using System.Linq;
using System.Runtime.Caching;

using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Caching.Memory
{
    /// <summary>
    /// Реализует интерфейс для управления кэшем в памяти.
    /// </summary>
    internal sealed class MemoryCacheImpl : IMemoryCache, IDisposable
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="appEnvironment">Пространство имен для ключей.</param>
        public MemoryCacheImpl(IAppEnvironment appEnvironment)
        {
            _cache = new MemoryCache(appEnvironment.Name);
        }


        private readonly MemoryCache _cache;


        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _cache.Contains(key);
        }

        public string Get(string key)
        {
            string value;

            TryGet(key, out value);

            return value;
        }

        public bool TryGet(string key, out string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            value = (string)_cache.Get(key);

            return (value != null);
        }

        public void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _cache.Set(key, value, new CacheItemPolicy());
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return (_cache.Remove(key) != null);
        }

        public void Clear()
        {
            foreach (var item in _cache.ToArray())
            {
                _cache.Remove(item.Key);
            }
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}