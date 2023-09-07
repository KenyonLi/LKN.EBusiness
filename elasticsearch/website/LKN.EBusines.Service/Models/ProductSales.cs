namespace LKN.EBusines.Service.Models
{
    /// <summary>
    /// 商品销售
    /// </summary>
    public class ProductSales
    {
        public int Id { set; get; } // 主键

        public int SoldCount { set; get; } // 已售件数

        public int PresellCount { set; get; } // 预售件数
    }
}
