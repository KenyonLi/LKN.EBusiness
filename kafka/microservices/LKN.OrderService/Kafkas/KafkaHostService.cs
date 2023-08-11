using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LKN.OrderService.Rabbitmqs
{
    public class KafkaHostService : IHostedService
    {
        private readonly IDistributedCache distributedCache;

        public KafkaHostService( IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
           /* new Task(() =>
            {*/
                // 1、创建连接
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = "127.0.0.1:9092",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    GroupId = "test1",
                    EnableAutoCommit = true,
                    FetchMinBytes = 2000,
                };
                var builder = new ConsumerBuilder<string, string>(consumerConfig);
                using (var Consumer = builder.Build())
                {
                   // Consumer.Seek(new TopicPartitionOffset(new TopicPartition("order-create",new Partition()), 8));
                    // 1、订阅
                    Consumer.Subscribe("order-create");
                    // 2、偏移量恢复
                    string offset = distributedCache.GetString("order-create");
                    if (string.IsNullOrEmpty(offset))
                    {
                        offset = "0";
                    }
                    Consumer.Assign(new TopicPartitionOffset(new TopicPartition("order-create", 0), int.Parse(offset)));
                    Console.WriteLine("kafka开始监听......");
                    while (true)
                    {
                        // 1、恢复消息
                        new Timer((s) => {
                             Consumer.Resume(new List<TopicPartition> { new TopicPartition("order-create", 0) });
                        }, null, Timeout.Infinite, Timeout.Infinite).Change(5000, 5000);

                        // 1.1、暂停消费
                        Consumer.Pause(new List<TopicPartition> { new TopicPartition("order-create", 0) });

                    // 2、消费
                    var result = Consumer.Consume();
                    try
                        {
                            Console.WriteLine($"订单消息偏移量：Offset:{result.Offset}");

                            // 2.1 存储偏移量
                            distributedCache.SetString("order-create", result.Offset.Value.ToString());

                            // 3、业务逻辑
                            string key = result.Key;
                            string value = result.Value;

                            Console.WriteLine($"创建商品：Key:{key}");
                            Console.WriteLine($"创建商品：Order:{value}");
                    }
                        catch (Exception)
                        {

                            throw;
                        } finally
                        {
                            Consumer.Pause(new List<TopicPartition> { new TopicPartition("order-create", 0) });
                            Console.WriteLine($"暂停消费");
                         }
                        
                }
            }
            /*}).Start();*/
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // 1、关闭rabbitmq的连接
            return Task.CompletedTask;
        }
    }
}
