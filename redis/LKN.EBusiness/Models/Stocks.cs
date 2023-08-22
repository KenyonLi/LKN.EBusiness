namespace LKN.EBusiness.Models
{
    public partial class Stocks
    {
        public int Id { get; set; } // 秒杀编号
        public int Count { get; set; } // 秒杀商品数量 10 
        public string ProductName { get; set; } // 秒杀商品名称 手机
    }
}
