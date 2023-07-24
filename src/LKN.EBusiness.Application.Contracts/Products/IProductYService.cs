using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.Products
{
    public interface IProductYService
    {
        IEnumerable<ProductDto> GetProducts();
        ProductDto GetProductById(Guid id);
        void CreateProduct(CreateProductDto createProductDto);
        void UpdateProduct(UpdateProductDto updateProductDto);
        void DeleteProduct(DeleteProductDto deleteProductDto);
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
