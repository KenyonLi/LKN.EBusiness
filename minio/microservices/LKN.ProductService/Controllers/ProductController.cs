using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YDT.ProductService.Models;

namespace YDT.ProductService.Controllers
{
    /// <summary>
    /// 商品控制器
    /// </summary>
    [ApiController]
    [Route("Product")]
    public class ProductController : ControllerBase
    {
        private static readonly string[] Products = new[]
        {
            "水果", "沙拉", "苹果", "梨子 ", "桃子", "核桃", "芒果", "车厘子", "樱桃", "橙子"
        };

        private readonly ILogger<ProductController> _logger;
        private readonly IConnection _connection;

        public ProductController(ILogger<ProductController> logger, IConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            _logger.LogInformation("查询商品信息");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Products[rng.Next(Products.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 扣减商品库存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ProductStock SubStock(OrderStockDto orderStockDto)
        {
            _logger.LogInformation("扣减商品库存");
            // 1、设置商品库存
            ProductStock productStock = new ProductStock()
            {
                Id = 1,
                ProductId = 1,
                Stock = 10
            };

            // 1、创建连接
            /* var factory = new ConnectionFactory()
             {
                 HostName = "localhost",
                 Port = 5672,
                 Password = "guest",
                 UserName = "guest",
                 VirtualHost = "/"
             };
             using (var connection = factory.CreateConnection())*/
            
            // 7、AMQP协议：高级消息协议
            return productStock;
        }
    }
}
