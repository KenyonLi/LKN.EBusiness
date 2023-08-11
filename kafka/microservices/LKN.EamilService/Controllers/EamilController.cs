using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKN.EamilService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EamilController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<EamilController> _logger;

        public EamilController(ILogger<EamilController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
           /* // 1、创建连接
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
                channel.ExchangeDeclare(exchange: "product_fanout", type: "fanout");

                // 2、定义随机队
                var queueName = channel.QueueDeclare().QueueName;

                // 3、队列要和交换机绑定起来
                channel.QueueBind(queueName,
                                     "product_fanout",
                                     routingKey: "");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    Console.WriteLine($"model:{model}");
                    var body = ea.Body;
                    // 1、业务逻辑
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] 发送邮件 {0}", message);

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


            #region 1、直连交换机
            {
                // 1、创建连接
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
                channel.ExchangeDeclare(exchange: "product_direct",
                                     type: "direct");

                // 2、定义随机队
                var queueName = channel.QueueDeclare().QueueName;

                // 3、队列要和交换机绑定起来
                channel.QueueBind(queueName,
                                     "product_direct",
                                     routingKey: "product-eamil");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    Console.WriteLine($"model:{model}");
                    var body = ea.Body;
                    // 1、业务逻辑
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] 发送邮件 {0}", message);

                    // 自动确认机制缺陷：
                    // 1、消息是否正常添加到数据库当中,所以需要使用手工确认
                    channel.BasicAck(ea.DeliveryTag, true);
                };
                // 3、消费消息
                channel.BasicQos(0, 1, false); // Qos(防止多个消费者，能力不一致，导致的系统质量问题。
                                               // 每一次一个消费者只成功消费一个)
                channel.BasicConsume(queue: queueName,
                                     autoAck: false, // 消息确认(防止消息消费失败)
                                     consumer: consumer);
            }
            #endregion

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
