using LKN.EBusiness.Models;
using LKN.EBusiness.Services;
using Microsoft.AspNetCore.Mvc;

namespace LKN.EBusiness.Controllers
{
    /// <summary>
    /// 商品控制器
    /// </summary>
    [ApiController]
    //[Route("Product")]
    [Route("[Controller]/[action]")]
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
        /// 添加商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Product> CreateProduct(Product product)
        {
            _productService.Create(product);
            return CreatedAtAction("GetProduct", "Product", new { id = product.Id }, product);
        }

        /// <summary>
        /// 批量添加商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult<Product> CreateProductList(Product[] Products)
        {
            _productService.CreateList(Products.ToList());
            return CreatedAtAction("GetProducts", "Product", Products);
        }

        /// <summary>
        /// 查询商品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return _productService.GetProducts().ToList();
        }

        /// <summary>
        /// 商品单个商品
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
        /// 商品分页查询
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        public ActionResult<IEnumerable<Product>> GetProductsByPage(int Page, int PageSize)
        {
            return _productService.GetProductsByPage(Page, PageSize).ToList();
        }

        [HttpGet("Aggregation")]
        public ActionResult<IEnumerable<int>> GetProductsByAggregation(Product product)
        {
            return _productService.GetProductsByAggregation(product).ToList();
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
            try
            {
                _productService.Update(id, product);
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("UpdateField")]
        public IActionResult PutFieldProduct(string id, ProductUpdateFiledDto productUpdateFiledDto)
        {
            try
            {
                _productService.UpdateFiled(id, productUpdateFiledDto);
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// 商品文档批量修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("UpdateList")]
        public IActionResult PutProductList(string id, Product product)
        {
            try
            {
                _productService.UpdateList(id, product);
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
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeletetProduct(Product product)
        {
            try
            {
                _productService.Delete(product);
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        // PUT: api/Products/5
        [HttpPost("CreateIndex")]
        public IActionResult CreateIndex()
        {

            try
            {
                _productService.CreateIndex();
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

    }
}

