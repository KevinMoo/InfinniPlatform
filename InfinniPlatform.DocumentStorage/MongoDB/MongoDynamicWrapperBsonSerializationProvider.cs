﻿using System;

using InfinniPlatform.Sdk.Dynamic;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет интерфейс сериализации и десериализации <see cref="DynamicWrapper"/> для MongoDB.
    /// </summary>
    internal sealed class MongoDynamicWrapperBsonSerializationProvider : IBsonSerializationProvider
    {
        public static readonly MongoDynamicWrapperBsonSerializationProvider Default = new MongoDynamicWrapperBsonSerializationProvider();


        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(DynamicWrapper))
            {
                return MongoDynamicWrapperBsonSerializer.Default;
            }

            return null;
        }
    }
}