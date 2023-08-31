namespace LKN.EBusiness.Models
{
    /// <summary>
    /// 商品MongoDB配置选项
    /// </summary>
    public class ProductMongoDBOptions
    {
        /// <summary>
        /// 商品集合名
        /// </summary>
        public string ProductCollectionName { get; set; }
        /// <summary>
        /// 连接mongodb字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 商品数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
