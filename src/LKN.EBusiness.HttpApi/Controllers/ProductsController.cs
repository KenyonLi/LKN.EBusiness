using LKN.EBusiness.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LKN.EBusiness.Controllers
{
    /// <summary>
    /// 商品服务控制器
    /// 1、复用
    /// 2、自定义
    /// </summary>
    [Route("Products")]
    [ApiController]
    public class ProductsController : AbpController
    {
        private readonly IProductService _productService;

        public IProductAppService _ProductAppService { set; get; } // 直接用ABP框架提供的Service

        public ProductsController(IProductService productService)
        {
            this._productService = productService;

        }

        // GET: api/Products
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetProducts()
        {
            //1、自己写的查询
            // return _productService.GetProducts().ToList();
            // 2、框架提供查询
            return _ProductAppService.GetListAsync(new PagedAndSortedResultRequestDto()).Result.Items.ToList();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProduct(Guid id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutProduct(UpdateProductDto updateProduct)
        {
            _productService.Update(updateProduct);

            return NoContent();
        }


        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<ProductDto> PostProduct(CreateProductDto createProductDto)
        {
            _productService.Create(createProductDto);
            return CreatedAtAction("GetProduct", createProductDto);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public ActionResult<ProductDto> DeleteProduct(Guid id)
        {
            /*var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productService.Delete(product);*/
            return null;
        }


        [HttpPost("{ProductId}/AddProductImage")]
        public ActionResult<ProductDto> PostProductImage(Guid ProductId, ProductImageCreateDto productImageCreateDto)
        {
            _productService.CreateProductImage(ProductId, productImageCreateDto);
            return null;
        }

        private bool ProductExists(Guid id)
        {
            return _productService.ProductExists(id);
        }

    }
}
