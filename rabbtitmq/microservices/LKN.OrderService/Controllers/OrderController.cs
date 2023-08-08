using LKN.OrderService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKN.OrderService.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    [ApiController]
    [Route("Order")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Order> Get()
        {
            // 1、创建订单
            Order order = new Order();

            // 2.1 发布扣减库存信息
            OrderStockDto orderStockDto = new OrderStockDto()
            {
                ProductId = 1,
                Stock = 1
            };

            #region 1、主题消费：发送到指定的多个队列消费
            {
                // 1、定义连接对象
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    Port = 5672,
                    Password = "guest",
                    UserName = "guest",
                    VirtualHost = "/"
                };
                var connection = factory.CreateConnection();
                // 工具：直连交换机 type:direct
                var channel = connection.CreateModel();
                // 1、定义交换机
                channel.ExchangeDeclare(exchange: "order_topic",
                                    type: "topic");
                // 2、定义随机队列(用完之后立马删除)
                var queueName = channel.QueueDeclare().QueueName;

                // 3、队列要和交换机绑定起来(#：匹配一个词和多个词，*：匹配一个词。)
                channel.QueueBind(queueName,
                                     "order_topic",
                                     routingKey: "sms.*");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    Console.WriteLine($"model:{model}");
                    var body = ea.Body;
                    // 1、执行业务
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] 创建商品 {0}", message);

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
                _logger.LogInformation("成功创建订单");
            }
            #endregion


            return order;
        }
    }
}
