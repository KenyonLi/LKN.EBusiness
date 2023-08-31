using LKN.EBusiness.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace LKN.EBusiness.MongoDBs
{
    /// <summary>
    /// MongoDB扩展方法
    /// </summary>
    public static class MongoDBServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services,IConfiguration Configuration)
        {
            // 1、获取配置文件数据
            var productMongoDBOptions =Configuration.GetSection(nameof(ProductMongoDBOptions)).Get<ProductMongoDBOptions>();

            // 1、建立MongoDB连接
            var client = new MongoClient(productMongoDBOptions.ConnectionString);

            // 2、获取商品库
           var database = client.GetDatabase(productMongoDBOptions.DatabaseName);

            // 3、获取商品表(集合)
            var _products = database.GetCollection<Product>(productMongoDBOptions.ProductCollectionName);

            services.AddSingleton(_products);

            return services;
        }
    }
}
