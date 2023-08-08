using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace LKN.ProductService.Rabbitmqs
{
    public class RabbitmqHostService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
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
            #region 1、工作队列(单消费者)
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
                    // 1、逻辑代码，添加商品到数据库
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] 创建商品 {0}", message);
                };

                channel.BasicConsume(queue: "product-create",
                                     autoAck: true, // 消息确认(防止消息重新消费)
                                     consumer: consumer);
            }
            #endregion

            Console.WriteLine("rabbitmq开始监听......");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // 1、关闭rabbitmq的连接
            throw new NotImplementedException();
        }
    }
}
