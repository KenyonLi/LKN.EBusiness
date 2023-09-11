using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YDT.ProductService.Models;

namespace YDT.OrderService.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    [ApiController]
    [Route("Order")]
    [Authorize] // 告诉cookie身份认证机制去验证电商网站是否安全
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
            OrderStockDto orderStockDto = new OrderStockDto() {
                ProductId = 1,
                Stock = 1
            };
            _logger.LogInformation("创建订单");
            return order;
        }
    }
}
