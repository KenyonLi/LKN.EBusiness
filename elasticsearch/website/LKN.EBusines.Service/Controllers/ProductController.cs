using LKN.EBusines.Service.Models;
using LKN.EBusines.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace LKN.EBusines.Service.Controllers
{
    /// <summary>
    /// 商品控制器
    /// </summary>
    [ApiController]
    [Route("Product")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        /// <summary>
        /// 查询商品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return _productService.GetProducts().ToList();
        }


        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(string id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// 查询商品(分页查询)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ProductsPage")]
        public ActionResult<IEnumerable<Product>> GetProductsByPage(int page, int pageSize)
        {
            var products = _productService.GetProductsByPage(page, pageSize).ToList();

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        /// <summary>
        /// 字段查询
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpGet("KeywordSearch")]
        public ActionResult<IEnumerable<Product>> GetProductsKeywordSearch([FromQuery] ProductDto productDto)
        {
            var products = _productService.GetProductsKeywordSearch(productDto);

            if (products == null)
            {
                return NotFound();
            }

            return products.ToList();
        }

        /// <summary>
        /// 全文搜索
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpGet("TextSearch")]
        public ActionResult<IEnumerable<Product>> GetProductsTextSearch([FromQuery] ProductDto productDto)
        {
            var products = _productService.GetProductsTextSearch(productDto);

            if (products == null)
            {
                return NotFound();
            }

            return products.ToList();
        }

        /// <summary>
        /// 聚合查询
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpGet("AggreateTextSearch")]
        public ActionResult<ValueAggregate> GetProductsAggreateTextSearch([FromQuery] ProductDto productDto)
        {
            var products = _productService.GetProductsAggreateTextSearch(productDto);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Product> PostProduct(Product product)
        {
            _productService.Create(product);
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult PutProduct(string id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                _productService.Update(product);
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id)
        {

            try
            {
                _productService.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }
    }
}
