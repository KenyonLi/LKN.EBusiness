using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 更新商品Dto
    /// </summary>
    public class UpdateProductDto
    {
        public string ProductUrl { set; get; }         // 商品主图
        public string ProductTitle { set; get; }       //商品标题
        public string ProductDescription { set; get; }     // 图文描述
        public decimal ProductVirtualprice { set; get; } //商品虚拟价格
        public decimal ProductPrice { set; get; }       //价格
    }
}
