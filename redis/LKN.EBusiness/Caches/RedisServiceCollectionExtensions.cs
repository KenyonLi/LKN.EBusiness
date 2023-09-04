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

            //ConfigurationOptions sentinelOptions = new ConfigurationOptions();
            //sentinelOptions.EndPoints.Add("192.168.1.46", 6380);
            //sentinelOptions.EndPoints.Add("192.168.1.46", 6381);
            //sentinelOptions.EndPoints.Add("192.168.1.46", 6382);
            //sentinelOptions.EndPoints.Add("192.168.1.46", 6383);
            //sentinelOptions.EndPoints.Add("192.168.1.46", 6384);
            //sentinelOptions.EndPoints.Add("192.168.1.46", 6385);
            //sentinelOptions.TieBreaker = "";
            //sentinelOptions.CommandMap = CommandMap.Create(new HashSet<string> {
            //       "INFO", "CONFIG", "CLUSTER",
            //      "PING", "ECHO", "CLIENT"
            //},available:false);
            //sentinelOptions.DefaultVersion = new Version(7,2,0);
            //sentinelOptions.AbortOnConnectFail = false;

            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);

            services.AddSingleton(connectionMultiplexer);
            return services;
        }
    }
}
