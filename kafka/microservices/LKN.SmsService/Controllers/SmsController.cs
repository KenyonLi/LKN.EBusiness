using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKN.SmsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SmsController> _logger;
        private readonly IDistributedCache distributedCache;
        public SmsController(ILogger<SmsController> logger/*, IDistributedCache distributedCache*/)
        {
            _logger = logger;
            /*this.distributedCache = distributedCache;*/
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            new Task(() =>
            {
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = "127.0.0.1:9092",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    GroupId = "sms",
                    EnableAutoCommit = false,
                };
                var builder = new ConsumerBuilder<string, string>(consumerConfig);
                var consumer = builder.Build();

                // 1、订阅
                consumer.Subscribe("create-order");
                while (true)
                {
                    // 2、消费
                    var result = consumer.Consume();
                    // 2.1、获取偏移量
                    _logger.LogInformation($"订单消息偏移量：Offset:{result.Offset}");

                    // 3、业务处理
                    string key = result.Key;
                    string value = result.Value;
                    _logger.LogInformation($"创建商品：Key:{key}");
                    _logger.LogInformation($"创建商品：Order:{value}");

                    // 3、手动提交
                    consumer.Commit(result);
                }
            }).Start();

            #region 1、多主题消费
            {
                /*new Task(() =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        GroupId = "sms",
                        EnableAutoCommit = false,
                    };
                    var builder = new ConsumerBuilder<string, string>(consumerConfig);
                    var consumer = builder.Build();
                    // 1、订阅
                    consumer.Subscribe(new List<string> {
                        "order-create",
                        "product-create"
                    });
                    while (true)
                    {
                        // 2、消费
                        var result = consumer.Consume();
                        string key = result.Key;
                        string value = result.Value;

                        _logger.LogInformation($"创建商品：Key:{key}");
                        _logger.LogInformation($"创建商品：Order:{value}");

                        // 3、手动提交
                        //consumer.Commit(result);

                        // 手动提交
                        consumer.StoreOffset(result);
                    }
                }).Start();*/
            }
            #endregion

            #region 2、主题交换机
            {
                /*// 1、创建连接
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    Port = 5672,
                    Password = "guest",
                    UserName = "guest",
                    VirtualHost = "/"
                };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                // 1、定义交换机
                channel.ExchangeDeclare(exchange: "sms_topic", type: ExchangeType.Topic);

                // 2、定义随机队列
                var queueName = channel.QueueDeclare().QueueName;

                // 3、队列要和交换机绑定起来。多对1
                // * 号的缺陷：只能匹配一级
                // # 能够匹配一级及多级以上
                // 真实项目当中，使用主题交换机。因为：可以满足所有场景
                channel.QueueBind(queueName,
                                      "sms_topic",
                                      routingKey: "sms.#");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    Console.WriteLine($"model:{model}");
                    var body = ea.Body;
                    // 1、业务逻辑
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] 发送短信 {0}", message);

                    // 自动确认机制缺陷：
                    // 1、消息是否正常添加到数据库当中,所以需要使用手工确认
                    channel.BasicAck(ea.DeliveryTag, true);
                };
                // 3、消费消息
                channel.BasicQos(0, 1, false); // Qos(防止多个消费者，能力不一致，导致的系统质量问题。
                                               // 每一次一个消费者只成功消费一个)
                channel.BasicConsume(queue: queueName,
                                     autoAck: false, // 消息确认(防止消息消费失败)
                                     consumer: consumer);*/
            }
            #endregion

            Console.WriteLine("短信创建监听......");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
