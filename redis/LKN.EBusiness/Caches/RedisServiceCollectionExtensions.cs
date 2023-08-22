using StackExchange.Redis;

namespace LKN.EBusiness.Caches
{
    public static class RedisServiceCollectionExtensions
    {
        /// <summary>
        /// 注册分布式redis 缓存 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedRedisCache(this IServiceCollection services, string connectionString)
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);

            services.AddSingleton(connectionMultiplexer);
            return services;
        }
    }
}
