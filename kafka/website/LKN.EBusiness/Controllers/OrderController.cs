using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LKN.ProductService.Models;

namespace LKN.EBusiness.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<OrderCreateDto> CreateOrder(OrderCreateDto orderCreateDto)
        {
            #region 1、生产者 Producer
            {
                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = "127.0.0.1:9092",
                    MessageTimeoutMs = 50000,
                    EnableIdempotence = true
                };

                var builder = new ProducerBuilder<string, string>(producerConfig);
                builder.SetDefaultPartitioner(RoundRobinPartitioner);
                using (var producer = builder.Build())
                {
                    try
                    {
                        var OrderJson = JsonConvert.SerializeObject(orderCreateDto);
                        //TopicPartition topicPartition = new TopicPartition("create-order", 1); // 指定分区发送消息
                        //var dr = producer.ProduceAsync(topicPartition, new Message<string, string> { Key = "order-1", Value = OrderJson }).GetAwaiter().GetResult();
                        var dr = producer.ProduceAsync("create-order", new Message<string, string> { Key = "order-1", Value = OrderJson }).GetAwaiter().GetResult();
                        _logger.LogInformation("发送事件 {0} 到 {1} 成功", dr.Value, dr.TopicPartitionOffset);
                    }
                    catch (ProduceException<string, string> ex)
                    {
                        _logger.LogError(ex, "发送事件到 {0} 失败，原因 {1} ", "order", ex.Error.Reason);
                    }
                }
            }
            #endregion

            #region 2、生产者-失败重试
            {
                /*var producerConfig = new ProducerConfig
                {
                    BootstrapServers = "127.0.0.1:9092",
                    MessageTimeoutMs = 50000,
                    EnableIdempotence = true // 保证消息：不重复发送，失败重试
                };

                var builder = new ProducerBuilder<string, string>(producerConfig);
                using (var producer = builder.Build())
                {
                    try
                    {
                        var OrderJson = JsonConvert.SerializeObject(orderCreateDto);
                        // TopicPartition topicPartition = new TopicPartition("order-create-5",new Partition(0));
                        var dr = producer.ProduceAsync("order-create-5", new Message<string, string> { Key = "order", Value = OrderJson }).GetAwaiter().GetResult();
                        _logger.LogInformation("发送事件 {0} 到 {1} 成功", dr.Value, dr.TopicPartitionOffset);
                    }
                    catch (ProduceException<string, string> ex)
                    {
                        _logger.LogError(ex, "发送事件到 {0} 失败，原因 {1} ", "order", ex.Error.Reason);
                    }
                }*/
            }
            #endregion

            #region 3、生产者-失败重试-多消息发送
            {
                //var producerConfig = new ProducerConfig
                //{
                //    BootstrapServers = "127.0.0.1:9092",
                //    MessageTimeoutMs = 50000,
                //    EnableIdempotence = true,
                //    TransactionalId = Guid.NewGuid().ToString()
                //};

                //var builder = new ProducerBuilder<string, string>(producerConfig);
                //using (var producer = builder.Build())
                //{
                //    // 1、初始化事务
                //    producer.InitTransactions(TimeSpan.FromSeconds(60));
                //    try
                //    {
                //        var OrderJson = JsonConvert.SerializeObject(orderCreateDto);
                //        // 2、开发事务
                //        producer.BeginTransaction();
                //        for (int i = 0; i < 100; i++)
                //        {
                //            var dr = producer.ProduceAsync("order-create-5", new Message<string, string> { Key = "order", Value = OrderJson }).GetAwaiter().GetResult();
                //            _logger.LogInformation("发送事件 {0} 到 {1} 成功", dr.Value, dr.TopicPartitionOffset);
                //        }
                //        // 3、提交事务
                //        producer.CommitTransaction();
                //    }
                //    catch (ProduceException<string, string> ex)
                //    {
                //        _logger.LogError(ex, "发送事件到 {0} 失败，原因 {1} ", "order", ex.Error.Reason);
                //        // 4、关闭事务
                //        producer.AbortTransaction();
                //    }
                //}
            }
            #endregion

            #region 4、生产者-固定分区发送
            {
                /*var producerConfig = new ProducerConfig
                {
                    BootstrapServers = "127.0.0.1:9092",
                    MessageTimeoutMs = 50000
                };
                var builder = new ProducerBuilder<string, string>(producerConfig);
                using (var producer = builder.Build())
                {
                    try
                    {
                        var OrderJson = JsonConvert.SerializeObject(orderCreateDto);
                        //TopicPartition topicPartition = new TopicPartition("order-create", new Partition(0));
                        var dr = producer.ProduceAsync("order-create", new Message<string, string> { Key = "order", Value = OrderJson }).GetAwaiter().GetResult();
                        _logger.LogInformation("发送事件 {0} 到 {1} 成功", dr.Value, dr.TopicPartitionOffset);
                    }
                    catch (ProduceException<string, string> ex)
                    {
                        _logger.LogError(ex, "发送事件到 {0} 失败，原因 {1} ", "order", ex.Error.Reason);
                    }
                }*/
            }
            #endregion

            return null;
        }

        /// <summary>
        /// 分区随机算法
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partitionCount"></param>
        /// <param name="keyData"></param>
        /// <param name="keyIsNull"></param>
        /// <returns></returns>
        private Partition RandomPartitioner(string topic, int partitionCount, ReadOnlySpan<byte> keyData, bool keyIsNull)
        {
            Random random = new Random();
            int partition = random.Next(partitionCount-1);
            return new Partition(partition);
        }

        /// <summary>
        /// 分区轮询算法。两个分区得到消息是一致的
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partitionCount"></param>
        /// <param name="keyData"></param>
        /// <param name="keyIsNull"></param>
        /// <returns></returns>
        static int requestCount = 0;
        private Partition RoundRobinPartitioner(string topic, int partitionCount, ReadOnlySpan<byte> keyData, bool keyIsNull)
        {
            int partition = requestCount % partitionCount;
            requestCount++;
            return new Partition(partition);
        }
    }
}
