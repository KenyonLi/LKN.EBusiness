using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LKN.Order.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 超时订单回收
        /// </summary>
        /// <returns></returns>
        [HttpPost("productOrder")]
        public IActionResult OrderCancel()
        {
            //1、超时订单回收
            _logger.LogInformation("回收超时订单");

            return Ok();
        }
    }
}
