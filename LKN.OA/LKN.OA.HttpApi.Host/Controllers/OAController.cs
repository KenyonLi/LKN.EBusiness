using LKN.EBusiness.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace LKN.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAController : ControllerBase
    {

        public IProductAppService _productAppService { set; get; }

        /// <summary>
        /// 1、获取商品API
        /// 为什么要使用动态API客户端调用？
        /// 总结：如果使用的ABP vNext HttpClient开发的项目，就必须使用动态API客户端远程。
        ///        提升开发的项目
        /// 动态API客户端 如何实现？已经能够理解
        /// 动态API客户端 缺陷？
        /// 1、只能C#语音调用。不能夸语言
        /// 2、如果OA是Java，就无法调用了。
        ///     gRPC来实现。
        ///     
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PagedResultDto<ProductDto> Get()
        {
            #region 1、httpClient调用
            {
                /*//1、查询请求
                var httpResponseMessage = _httpClient.GetAsync("https://localhost:44389/api/EBusiness/product").Result;
                string productContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

                // 2、json转换成对象
                var products = JsonConvert.DeserializeObject<PagedResultDto<ProductDto>>(productContent);
                return products;*/

            }
            #endregion

            #region 2、ProductService直接调用
            {
                //1、查询请求
               // return _productService.GetProducts();
            }
            #endregion


            #region 2、动态API调用
            {
                
                
                var products = _productAppService.GetListAsync(new PagedAndSortedResultRequestDto()).Result;
                return products;
                // 1、不能夸语言
            }
            #endregion

        }
    }
}
