using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace LKN.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IConnection _connection;
        public ProductController(ILogger<ProductController> logger) {
            _logger = logger; 
        }
        /// <summary>
        /// 创建商品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<Product> CreateProducts() {

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
            #region 1、工作队列(单消费者)
            {
                //var channel = connection.CreateModel();

                //// 2、定义队列
                //channel.QueueDeclare(queue: "product-create",
                //                     durable: false,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);
                //// 3、发送消息
                //var properties = channel.CreateBasicProperties();
                //properties.Persistent = true; // 设置消息持久化（个性化控制）

                //var consumer = new EventingBasicConsumer(channel);
                //consumer.Received += (model, ea) =>
                //{

                //    Console.WriteLine($"model:{model}");
                //    var body = ea.Body;
                //    // 1、逻辑代码，添加商品到数据库
                //    var message = Encoding.UTF8.GetString(body.ToArray());
                //    Console.WriteLine(" [x] 创建商品 {0}", message);
                //};

                //channel.BasicConsume(queue: "product-create",
                //                     autoAck: true, // 自动消息确认
                //                     consumer: consumer);
            }
            #endregion

            #region 2、工作队列(单消费者)-手工确认消息
            {
                var channel = connection.CreateModel();

                // 2、定义队列
                channel.QueueDeclare(queue: "product-create",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {

                    Console.WriteLine($"model:{model}");
                    var body = ea.Body;
                    // 1、逻辑代码
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] 创建商品 {0}", message);

                    // 自动确认机制缺陷：
                    // 1、消息是否正常添加到数据库当中,所以需要使用手工确认
                    channel.BasicAck(ea.DeliveryTag, true);
                };
                channel.BasicConsume(queue: "product-create",
                                     autoAck: false, // 消息确认(防止消息重新消费)
                                     consumer: consumer);
            }
            #endregion

            #region 4、工作队列(单消费者)-手工确认消息-消息持久化
            {
                /* var channel = connection.CreateModel();

                 // 2、定义队列
                 channel.QueueDeclare(queue: "product-create",
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                 var consumer = new EventingBasicConsumer(channel);
                 consumer.Received += (model, ea) =>
                 {
                     Console.WriteLine($"model:{model}");
                     var body = ea.Body;
                     // 1、逻辑代码
                     var message = Encoding.UTF8.GetString(body.ToArray());

                     Console.WriteLine(" [x] 创建商品 {0}", message);

                     // 自动确认机制缺陷：
                     // 1、消息是否正常添加到数据库当中,所以需要使用手工确认
                     channel.BasicAck(ea.DeliveryTag, true);
                 };

                 channel.BasicConsume(queue: "product-create",
                                      autoAck: false, // 消息确认(防止消息重新消费)
                                      consumer: consumer);*/
            }
            #endregion

            #region 5、工作队列(多消费者)-手工确认消息-消息持久化-消费者质量
            {
                //var channel = connection.CreateModel();

                //// 2、定义队列
                //channel.QueueDeclare(queue: "product-create",
                //                     durable: false,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);

                //var consumer = new EventingBasicConsumer(channel);
                //consumer.Received += (model, ea) =>
                //{
                //    Console.WriteLine($"model:{model}");
                //    var body = ea.Body;
                //    var message = Encoding.UTF8.GetString(body.ToArray());
                //    Console.WriteLine(" [x] 创建商品 {0}", message);

                //    // 自动确认机制缺陷：
                //    // 1、消息是否正常添加到数据库当中,所以需要使用手工确认
                //    channel.BasicAck(ea.DeliveryTag, true);
                //};
                //// 3、消费消息
                //channel.BasicQos(0, 1, false); // Qos(防止多个消费者，能力不一致，导致的系统质量问题。
                //                               // 每一次一个消费者只成功消费一个)
                //channel.BasicConsume(queue: "product-create",
                //                     autoAck: false, // 消息确认(防止消息消费失败)
                //                     consumer: consumer);
            }
            #endregion

            // 队列工作机理
            // 1、需要使用交换机
            // 1、切换场景
            // 创建订单，发送短信。

            #region 6、订阅发布(广播消费)1、创建商品----2、发送短信-扇形交换机
            {
                /*var channel = connection.CreateModel();

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
                                     consumer: consumer);*/
            }
            #endregion

            // 需要使用路由Key
            // 1、routing key
            // 2、Binding
            // 场景：创建商品，指定发送短信或者发送邮件
            #region 7、创建商品----2、发送短信或者发送邮件--直连交换机
            {
                /*// 工具：直连交换机 type:direct
                var channel = connection.CreateModel();
                // 1、定义交换机
                channel.ExchangeDeclare(exchange: "product_direct",
                                    type: "direct");
               *//* // 2、定义随机队列(用完之后立马删除)
                var queueName = channel.QueueDeclare().QueueName;

                // 3、队列要和交换机绑定起来
                channel.QueueBind(queueName,
                                     "product_direct",
                                     routingKey: "product-sms");*//*

                // 2、定义随机队列(用完之后立马删除)
                var queueName1 = channel.QueueDeclare().QueueName;

                // 3、队列要和交换机绑定起来
                channel.QueueBind(queueName1,
                                     "product_direct",
                                     routingKey: "product-eamil");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    Console.WriteLine($"model:{model}");
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    // Thread.Sleep(1000);
                    Console.WriteLine(" [x] 创建商品 {0}", message);
                };
                // 3、消费消息
                channel.BasicQos(0, 1, false); // Qos(防止多个消费者，能力不一致，导致的系统质量问题。
                                               // 每一次一个消费者只成功消费一个)
                channel.BasicConsume(queue: queueName1,
                                     autoAck: true, // 消息确认(防止消息消费失败)
                                     consumer: consumer);*/
            }
            #endregion

            // 场景：创建订单，也希望发送短信
            #region 8、创建商品(创建订单)---- 2、发送短信-主题交换机
            {
                /* // 工具：直连交换机 type:direct
                 var channel = connection.CreateModel();
                 // 1、定义交换机
                 channel.ExchangeDeclare(exchange: "product_topic",
                                     type: "topic");
                 // 2、定义随机队列(用完之后立马删除)
                 var queueName = channel.QueueDeclare().QueueName;

                 // 3、队列要和交换机绑定起来(#：匹配一个词和多个词，*：匹配一个词。)
                 channel.QueueBind(queueName,
                                      "product_topic",
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
                                      consumer: consumer);*/
            }
            #endregion

            // 场景：添加商品是否成功
            #region 9、创建商品-----回调-RPC
            {
                // 工具：直连交换机 type:direct
                //var channel = connection.CreateModel();

                //// 1、定义随机队列(用完之后立马删除)
                //var queueName = channel.QueueDeclare(queue: "product_create2",
                //                                     durable: false,
                //                                     exclusive: false,
                //                                     autoDelete: false,
                //                                     arguments: null);

                //var consumer = new EventingBasicConsumer(channel);
                //consumer.Received += (model, ea) =>
                //{
                //    Console.WriteLine($"model:{model}");
                //    var body = ea.Body;

                //    var props = ea.BasicProperties;
                //    var replyProps = channel.CreateBasicProperties();
                //    replyProps.CorrelationId = props.CorrelationId;

                //    try
                //    {
                //        // 1、执行业务
                //        var message = Encoding.UTF8.GetString(body.ToArray());
                //        Console.WriteLine(" [x] 创建商品 {0}", message);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(" [.] " + e.Message);
                //    }
                //    finally
                //    {
                //        Console.WriteLine("发送回调消息");
                //        var responseBytes = Encoding.UTF8.GetBytes("商品回调成功");
                //        channel.BasicPublish(exchange: "",
                //                            routingKey: props.ReplyTo,
                //                            basicProperties: replyProps,
                //                            body: responseBytes);
                //        /*channel.BasicAck(deliveryTag: ea.DeliveryTag,
                //          multiple: false);*/
                //    }
                //};
                //// 3、消费消息
                //// channel.BasicQos(0, 1, false); // Qos(防止多个消费者，能力不一致，导致的系统质量问题。
                //// 每一次一个消费者只成功消费一个)
                //channel.BasicConsume(queue: "product_create2",
                //                     autoAck: true, // 消息确认(防止消息消费失败)
                //                     consumer: consumer);
            }
            #endregion

            //Console.WriteLine("商品创建监听......");
            return null;
        }
    }
}
