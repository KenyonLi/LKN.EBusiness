using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LKN.ProductService.Models;

namespace LKN.OrderService.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    [ApiController]
    [Route("Kafka")]
    public class KafkaController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        public KafkaController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// 创建分区(更新分区)
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="Partitions"></param>
        /// <returns></returns>
        [HttpGet("PartitionUpdate")]
        public async Task PartitionCreate(string topic,int PartitionCount)
        {
            AdminClientConfig adminClientConfig = new AdminClientConfig
            {
                BootstrapServers = "127.0.0.1:9092",
            };

            var bu = new AdminClientBuilder(adminClientConfig).Build();
            bu.CreatePartitionsAsync(new PartitionsSpecification[] {
                    new PartitionsSpecification { Topic = topic, IncreaseTo=PartitionCount}
                }).Wait();

            await Task.CompletedTask;
        }

        /// <summary>
        /// 创建主题
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="Partitions"></param>
        /// <returns></returns>
        [HttpGet("TopicCreate")]
        public async Task TopicCreate(string topic)
        {
            AdminClientConfig adminClientConfig = new AdminClientConfig
            {
                BootstrapServers = "127.0.0.1:9092",
            };

            var bu = new AdminClientBuilder(adminClientConfig).Build();
            bu.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification { Name = topic}
                }).Wait();

            await Task.CompletedTask;
        }

        /// <summary>
        /// 创建主题和分区
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="Partitions"></param>
        /// <returns></returns>
        [HttpGet("TopicPartitionCreate")]
        public async Task TopicPartitionCreate(string topic,int PartitionCount)
        {
            AdminClientConfig adminClientConfig = new AdminClientConfig
            {
                BootstrapServers = "127.0.0.1:9092",
            };

            var bu = new AdminClientBuilder(adminClientConfig).Build();
            bu.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification { Name = topic,NumPartitions =PartitionCount}
                }).Wait();

            await Task.CompletedTask;
        }
    }
}
