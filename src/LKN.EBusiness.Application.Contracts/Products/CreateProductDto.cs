using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 创建商品Dto
    /// 
    /// </summary>
    public class CreateProductDto
    {
        public string ProductUrl { set; get; }         // 商品主图
        public string ProductTitle { set; get; }       //商品标题
        public string ProductDescription { set; get; }     // 图文描述
        public decimal ProductVirtualprice { set; get; } //商品虚拟价格
        public decimal ProductPrice { set; get; }       //价格

        public ProductImageCreateDto[] ProductImages { set; get; } // 商品图片集合
    }

    public class ProductImageCreateDto
    {
        public ProductImageCreateDto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { set; get; } // Guid
        public string ImageUrl { set; get; } // 图片url
        public string ImageStatus { set; get; } // 图片状态
    }
}
