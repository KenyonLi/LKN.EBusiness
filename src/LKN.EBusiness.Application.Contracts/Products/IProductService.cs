using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.Products
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetProducts();
        ProductDto GetProductById(Guid id);
        void Create(CreateProductDto createProductDto);
        void Update(UpdateProductDto updateProductDto);
        void Delete(DeleteProductDto deleteProductDto);
        bool ProductExists(Guid id);

        /// <summary>
        /// 添加商品图片
        /// </summary>
        /// <param name="productImageCreateDto"></param>
        public void CreateProductImage(Guid ProductId, ProductImageCreateDto productImageCreateDto);

        /// <summary>
        /// 删除指定商品下面的图片
        /// </summary>
        /// <param name="productImageCreateDto"></param>
        public void DeleteProductImage(Guid ProductId, DeleteProductImageDto deleteProductImageDto);

        public void GetProductByName();
    }
}
