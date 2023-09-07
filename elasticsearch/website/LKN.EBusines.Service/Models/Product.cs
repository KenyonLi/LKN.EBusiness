namespace LKN.EBusines.Service.Models
{
    /// <summary>
    /// 商品
    /// </summary>
    public class Product
    {
        public string Id { set; get; }
        public string ProductCode { set; get; }    //商品编码
        public string ProductUrl { set; get; }         // 商品主图 text
        public string ProductTitle { set; get; }       //商品标题
        public string ProductDescription { set; get; }     // 图文描述
        public decimal ProductVirtualprice { set; get; } // 商品虚拟价格
        public decimal ProductPrice { set; get; }       //价格
        public int ProductSort { set; get; }    //商品序号
        public int ProductSold { set; get; }        //已售件数
        public int ProductStock { set; get; }       //商品库存
        public string ProductStatus { set; get; } // 商品状态
        public int score { set; get; } //商品级别

        /// <summary>
        /// 商品图片
        /// </summary>
       // public List<ProductImage> productImages { set; get; } 

        /// <summary>
        /// 商品销售
        /// </summary>
       // public ProductSales productSales { set; get; }
    }
}
