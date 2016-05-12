﻿using System;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.MessageQueue.Tests
{
    public class QueueTestHttpService
    {
        public QueueTestHttpService(IProducer producer, IBasicConsumer basicConsumer)
        {
            _producer = producer;
            _basicConsumer = basicConsumer;
        }

        private readonly IBasicConsumer _basicConsumer;

        private readonly IProducer _producer;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/produce"] = Produce;
            builder.Get["/consume"] = Consume;
        }

        private Task<object> Produce(IHttpRequest request)
        {
            _producer.Produce("test_queue", new Message<TestMessage>(new TestMessage("string", 1, DateTime.Now)));

            return Task.FromResult<object>(new
                                           {
                                               IsValid = true,
                                               Message = "Message produced."
                                           });
        }

        private Task<object> Consume(IHttpRequest request)
        {
            var message = _basicConsumer.Consume<TestMessage>("test_queue");

            if (message == null)
            {
                return Task.FromResult<object>(new
                                               {
                                                   IsValid = false,
                                                   ErrorMessage = "No new messages."
                                               });
            }

            return Task.FromResult<object>(new
                                           {
                                               IsValid = true,
                                               Message = message
                                           });
        }
    }
}