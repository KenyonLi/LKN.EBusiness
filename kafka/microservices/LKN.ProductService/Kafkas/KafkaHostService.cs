using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LKN.ProductService.Rabbitmqs
{
    public class KafkaHostService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 1、创建连接
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = Guid.NewGuid().ToString(),
                EnableAutoCommit = true,
            };
            var builder = new ConsumerBuilder<string, string>(consumerConfig);
            var consumer = builder.Build();
            // 1、订阅
            consumer.Subscribe("order-create");
            while (true)
            {
                // 2、消费
                var result = consumer.Consume();
                string key = result.Key;
                string value = result.Value;

                Console.WriteLine($"key:{key}");
                Console.WriteLine($"order:{value}");
            }

            Console.WriteLine("kafka开始监听......");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // 1、关闭rabbitmq的连接
            throw new NotImplementedException();
        }
    }
}
