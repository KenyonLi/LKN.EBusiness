using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKN.EBusiness.Services
{
    public class MessageConnection
    {
        public IConnection GetConnection()
        {
            // 如何防止重复创建多个连接？
            // 导致问题：连接数不够。
            // 连接池。如何实现一个连接池。
            // 工具：享元模式。 
            // IOC容器。数据库连接池
            // 23种设计模式
            // 工具：写源码
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                Password = "guest",
                UserName = "guest",
                VirtualHost = "/"
            };
            return factory.CreateConnection();
        }
    }
}
