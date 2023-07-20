using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 删除商品Dto
    /// </summary>
    public class DeleteProductDto
    {
        public string ProductUrl { set; get; }         // 商品主图
        public string ProductTitle { set; get; }       //商品标题
        public string ProductDescription { set; get; }     // 图文描述
        public decimal ProductVirtualprice { set; get; } //商品虚拟价格
        public decimal ProductPrice { set; get; }       //价格
    }

    public class DeleteProductImageDto
    {
        public Guid ProductImageId { set; get; }         // 商品图片
    }
}
