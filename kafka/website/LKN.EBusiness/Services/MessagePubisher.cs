using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKN.EBusiness.Services
{
    /// <summary>
    /// 消息发布类
    /// </summary>
    public class MessagePubisher
    {
        public MessageConnection messageConnection { set; get; }

        public void PublishMessage(string Queue, object Message)
        {
            // 如何解决复用连接的问题？
            /*var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                Password = "guest",
                UserName = "guest",
                VirtualHost = "/"
            };*/
            using (var connection = messageConnection.GetConnection())
            {
                var channel = connection.CreateModel();
                // 2、定义队列
                channel.QueueDeclare(queue: Queue,
                                     durable: false,// 消息持久化(防止rabbitmq宕机导致队列丢失风险)
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string productJson = JsonConvert.SerializeObject(Message);
                // string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(productJson);

                // 3、发送消息
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true; // 设置消息持久化
                channel.BasicPublish(exchange: "",
                                     routingKey: Queue,
                                     basicProperties: properties,
                                     body: body);
            }
        }
    }
}
