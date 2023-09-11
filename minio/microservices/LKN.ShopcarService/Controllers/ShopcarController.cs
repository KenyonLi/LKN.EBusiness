using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace YDT.ShopcarService.Controllers
{
    /// <summary>
    /// 购物车控制器
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ShopcarController : ControllerBase
    {
        private readonly ILogger<ShopcarController> _logger;
        public ShopcarController(ILogger<ShopcarController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            #region 1.Dapr HTTP客户端请求
            {
                // DaprClient
                // 1、创建
                // 2、使用
                // 1、创建Dapr客户端

                // 2、访问商品微服务
               
                return null;
            }
            #endregion

            #region 2.Http方式请求
            {
                /* // 1、创建Dapr客户端
                 var client = DaprClient.CreateInvokeHttpClient(appId: "productservice");

                 // 2、访问商品微服务
                 var result = client.GetFromJsonAsync<dynamic>("/WeatherForecast").Result;
                 Console.WriteLine($"aaa:{result}");

                 // 3、将json转换成对象输出
                 //var content = JsonSerializer.Deserialize<List<WeatherForecast>>(result);
                 return new List<WeatherForecast>();*/
            }
            #endregion

        }
    }
}
