namespace LKN.EBusines.Service.Models
{
    /// <summary>
    /// 商品MongoDB配置选项
    /// </summary>
    public class ProductMongoDBOptions
    {
        public string ProductCollectionName { get; set; } // 商品集合名
        public string ConnectionString { get; set; } // 连接Mongodb字符串
        public string DatabaseName { get; set; } // 商品数据库名
    }
}
