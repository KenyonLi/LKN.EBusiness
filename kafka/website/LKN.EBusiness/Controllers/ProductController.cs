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
    public class ProductController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 创建商品
        /// </summary>
        /// <param name="productCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<Product> CreateProduct(ProductCreateDto productCreateDto)
        {
            #region 1、生产者
            {
                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = "127.0.0.1:9092",
                    MessageTimeoutMs = 50000
                };

                var builder = new ProducerBuilder<string, string>(producerConfig);
                using (var producer = builder.Build())
                {
                    try
                    {
                        var OrderJson = JsonConvert.SerializeObject(productCreateDto);
                        var dr = producer.ProduceAsync("product-create", new Message<string, string> { Key = "order", Value = OrderJson }).GetAwaiter().GetResult();
                        _logger.LogInformation("发送事件 {0} 到 {1} 成功", dr.Value, dr.TopicPartitionOffset);
                    }
                    catch (ProduceException<string, string> ex)
                    {
                        _logger.LogError(ex, "发送事件到 {0} 失败，原因 {1} ", "order", ex.Error.Reason);
                    }
                }
            }
            #endregion

            #region 2、扇形交换机
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    Port = 5672,
                    Password = "guest",
                    UserName = "guest",
                    VirtualHost = "/"
                };
                using (var connection = factory.CreateConnection())
                {
                    var channel = connection.CreateModel();
                    // 2、定义交换机
                    channel.ExchangeDeclare(exchange: "product_fanout", type: "fanout");

                    string productJson = JsonConvert.SerializeObject(productCreateDto);
                    // string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(productJson);

                    // 3、发送消息
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true; // 设置消息持久化
                    channel.BasicPublish(exchange: "product_fanout",
                                         routingKey: "",
                                         basicProperties: properties,
                                         body: body);
                }
                _logger.LogInformation("成功创建商品");
            }
            #endregion

            #region 3、直连交换机
            {
              /*  var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    Port = 5672,
                    Password = "guest",
                    UserName = "guest",
                    VirtualHost = "/"
                };
                using (var connection = factory.CreateConnection())
                {
                    var channel = connection.CreateModel();
                    // 2、定义交换机
                    channel.ExchangeDeclare(exchange: "product_direct", type: "direct");

                    string productJson = JsonConvert.SerializeObject(productCreateDto);
                    // string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(productJson);

                    // 3、发送消息
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true; // 设置消息持久化
                    channel.BasicPublish(exchange: "product_direct",
                                         routingKey: "product-eamil",
                                         basicProperties: properties,
                                         body: body);
                }
                _logger.LogInformation("成功创建商品");*/
            }
            #endregion

            #region 4、主题交换机
            {
               /* var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    Port = 5672,
                    Password = "guest",
                    UserName = "guest",
                    VirtualHost = "/"
                };
                using (var connection = factory.CreateConnection())
                {
                    var channel = connection.CreateModel();
                    // 2、定义交换机
                    channel.ExchangeDeclare(exchange: "sms_topic", type: "topic");

                    string productJson = JsonConvert.SerializeObject(productCreateDto);
                    // string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(productJson);

                    // 3、发送消息
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true; // 设置消息持久化
                    channel.BasicPublish(exchange: "sms_topic",
                                         routingKey: "sms.product.update",
                                         basicProperties: properties,
                                         body: body);
                }
                _logger.LogInformation("成功创建商品");*/
            }
            #endregion

            #region 3、RPC回调来实现
            {
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
                    // 2、定义队列
                    string replyQueueName = channel.QueueDeclare().QueueName;

                    var properties = channel.CreateBasicProperties();
                    var correlationId = Guid.NewGuid().ToString();
                    properties.CorrelationId = correlationId;
                    properties.ReplyTo = replyQueueName;

                    // 3、发送消息
                    string productJson = JsonConvert.SerializeObject(productCreateDto);
                    // string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(productJson);
                    properties.Persistent = true; // 设置消息持久化
                    channel.BasicPublish(exchange: "",
                                         routingKey: "product_create2",
                                         basicProperties: properties,
                                         body: body);

                    // 4、消息回调
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        Console.WriteLine($"model:{model}");
                        var body = ea.Body;
                        // 1、业务逻辑处理
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        if (ea.BasicProperties.CorrelationId == correlationId)
                        {
                            Console.WriteLine(" [x] 回调成功 {0}", message);
                        }

                    };
                    // 3、消费消息
                    // channel.BasicQos(0, 1, false); // Qos(防止多个消费者，能力不一致，导致的系统质量问题。
                    // 每一次一个消费者只成功消费一个)
                    channel.BasicConsume(queue: replyQueueName,
                                         autoAck: true, // 消息确认(防止消息消费失败)
                                         consumer: consumer);

                _logger.LogInformation("成功创建商品");
            }
            #endregion
            return null;
        }
    }
}
