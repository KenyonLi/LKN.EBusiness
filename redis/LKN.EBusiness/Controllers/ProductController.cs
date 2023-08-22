using LKN.EBusiness.Contexts;
using LKN.EBusiness.Locks;
using LKN.EBusiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LKN.EBusiness.Controllers
{
    /// <summary>
    /// 商品控制器
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductDbContext _productDbContext;
        private readonly IMemoryCache memoryCache;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly static object _lock = new object();// 创建静态锁
        public ProductController(ILogger<ProductController> logger,
                                 ProductDbContext productDbContext,
                                 ConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _productDbContext = productDbContext;
            _connectionMultiplexer = connectionMultiplexer;
        }


        /// <summary>
        /// 查询商品
        /// 
        /// 总结
        /// 1、根据具体商品数据，使用API查询数据
        ///     情况1：单个商品存储
        ///     情况2：多个商品存储
        ///     情况3：商品的字段更新
        ///     情况4：商品多个字段添加一致性
        ///     情况5：多个商品数据批量添加
        ///     情况6：商品数据如何排序
        ///     情况7：商品数据如何分页
        ///     
        ///      数据集群，分布式锁
        ///      1、AOF文件
        ///      2、集群原理，
        ///      3、分布式如何实现。
        ///      
        /// 海里数据缓存方案：SSDB。P7
        /// redis:只能根据内存大小存储。用redis内存空间换取查询时间算法
        /// 
        /// 如果多个项目：数据不共享：就不用redis，如果是共享的，就用redis。
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpGet]
        public Product GetProduct()
        {
            // 1、查询数据库数据
            Product product = _productDbContext.Products.FirstOrDefault(s => s.Id == 1);

            #region 1、存储商品对象，一条商品数据
            {
                /* // 1、从redis中取对象
                 string productjson = _connectionMultiplexer.GetDatabase(0).StringGet("product");
                 if (string.IsNullOrEmpty(productjson))
                 {
                     // 2、从数据库中查询
                     product = _productDbContext.Products.FirstOrDefault(s => s.Id == 1);

                     // 3、存储到redis中
                     string demojson = JsonConvert.SerializeObject(product);//序列化
                     _connectionMultiplexer.GetDatabase(0).StringSet("product", demojson);

                     return product;
                 }
                 product = JsonConvert.DeserializeObject<Product>(productjson);//反序列化

                 return product;*/
            }
            #endregion

            #region 2、存储商品对象-集合商品数据
            {
                /*// 1、从redis中取对象
                RedisValue[] productvalues = _connectionMultiplexer.GetDatabase(0).SetMembers("products");
                List<Product> products = new List<Product>();
                if (productvalues.Length == 0)
                {
                    // 2、从数据库中查询
                    products = _productDbContext.Products.ToList();

                    // 3、存储到redis中
                    List<RedisValue> redisValues = new List<RedisValue>();
                    foreach (var product1 in products)
                    {
                        string productjson = JsonConvert.SerializeObject(product1);//序列化
                        redisValues.Add(productjson);
                    }

                    _connectionMultiplexer.GetDatabase(0).SetAdd("products", redisValues.ToArray());

                    return products;
                }

                // 4、序列化，反序列化
                foreach (var redisValue in productvalues)
                {
                    product = JsonConvert.DeserializeObject<Product>(redisValue);//反序列化
                    products.Add(product);
                }
                return product;*/
            }
            #endregion

            #region 3、存储商品对象-集合-分页查询
            {
                /*// 1、从redis中取对象
                RedisValue[] productvalues = _connectionMultiplexer.GetDatabase(0).SetScan("products", 10, 0, 10).ToArray();
                List<Product> products = new List<Product>();
                if (productvalues.Length == 0)
                {
                    // 2、从数据库中查询
                    products = _productDbContext.Products.ToList();

                    // 3、存储到redis中
                    List<RedisValue> redisValues = new List<RedisValue>();
                    foreach (var product1 in products)
                    {
                        string productjson = JsonConvert.SerializeObject(product1);//序列化
                        redisValues.Add(productjson);
                    }

                    _connectionMultiplexer.GetDatabase(0).SetAdd("products", redisValues.ToArray());
                }

                // 4、序列化，反序列化
                foreach (var redisValue in productvalues)
                {
                    product = JsonConvert.DeserializeObject<Product>(redisValue);//反序列化
                    products.Add(product);
                }
                return product;*/
            }
            #endregion

            #region 4、存储商品对象-字典形式
            {
                string ProductSold = _connectionMultiplexer.GetDatabase(0).HashGet("productHash", "ProductSold");
                if (string.IsNullOrEmpty(ProductSold))
                {
                    product = _productDbContext.Products.FirstOrDefault(s => s.Id == 1);
                    _connectionMultiplexer.GetDatabase(0).HashSet("productHash", "ProductSold", product.ProductStock);
                   /// _connectionMultiplexer.GetDatabase(0).KeyExpire("productHash", TimeSpan.FromSeconds(10));
                }

                // 1、增加销量
                _connectionMultiplexer.GetDatabase(0).HashIncrement("productHash", "ProductSold");
                return product;
            }
            #endregion

            #region 5、存储商品对象-事务
            {
                /*string ProductSold = _connectionMultiplexer.GetDatabase(0).HashGet("productHash", "ProductSold");
                string ProductStock = _connectionMultiplexer.GetDatabase(0).HashGet("productHash", "ProductStock");
                if (string.IsNullOrEmpty(ProductStock))
                {
                    product = _productDbContext.Products.FirstOrDefault(s => s.Id == 1);

                    //创建事务
                    ITransaction transaction = _connectionMultiplexer.GetDatabase(0).CreateTransaction();
                    //transaction.AddCondition(Condition.HashEqual("productHash", "ProductSold", product.ProductSold));//乐观锁
                    transaction.HashSetAsync("productHash", "ProductSold", product.ProductSold);
                    transaction.HashSetAsync("productHash", "ProductStock", product.ProductStock);
                    //transaction.HashSetAsync("productHash", "ProductSold", 100); // 修改原油值
                    bool commit = transaction.Execute();
                    // commit 为true：那么就是提交成功 如果为false,提交失败
                    if (commit)
                    {
                        Console.WriteLine("提交成功");
                    }
                    else
                    {
                        Console.WriteLine("回滚成功");
                    }
                }
                return product;*/
            }
            #endregion

            #region 6、存储商品对象-批量操作
            {
                /*var batch = _connectionMultiplexer.GetDatabase(0).CreateBatch();

                List<Product> products = new List<Product>();
                // 2、从数据库中查询
                products = _productDbContext.Products.ToList();

                // 3、存储到redis中
                for (int i = 0; i < products.Count; i++)
                {
                    batch.HashSetAsync("productHash" + i, "ProductSold", products[i].ProductSold);
                }
                batch.Execute();

                return product;*/
            }
            #endregion

            #region 7、存储商品对象-消息队列-扩展
            {
            }
            #endregion

            #region 8、存储商品对象-订阅发布-扩展
            {
            }
            #endregion

            #region 9、存储商品对象-分布式锁
            {

            }
            #endregion

            #region 10、存储商品对象-集群
            {

            }
            #endregion

            return product;
        }


        /// <summary>
        /// 查询商品集合
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpGet("/ProductList")]
        public List<Product> GetProductList()
        {
            // redis---> set集合
            #region 2、存储商品对象-集合
            {
                // 1、从redis中取对象
                RedisValue[] productvalues = _connectionMultiplexer.GetDatabase(0).SetMembers("products");
                List<Product> products = new List<Product>();
                if (productvalues.Length == 0)
                {
                    // 2、从数据库中查询
                    products = _productDbContext.Products.ToList();

                    // 3、存储到redis中
                    List<RedisValue> redisValues = new List<RedisValue>();
                    foreach (var product1 in products)
                    {
                        string productjson = JsonConvert.SerializeObject(product1);//序列化
                        redisValues.Add(productjson);

                        // 对商品数据进行排序
                        _connectionMultiplexer.GetDatabase(0).SortedSetAdd("products", productjson, product1.ProductSold);
                    }

                    // _connectionMultiplexer.GetDatabase(0).SetAdd("products", redisValues.ToArray());

                    // 使用排序api
                    /*_connectionMultiplexer.GetDatabase(0).SortedSetAdd("products", "2",1);
                    _connectionMultiplexer.GetDatabase(0).SortedSetAdd("products", "3", 2);
                    _connectionMultiplexer.GetDatabase(0).SortedSetAdd("products", "4", 3);*/
                    return products;
                }

                // 4、序列化，反序列化
                foreach (var redisValue in productvalues)
                {
                    Product product = JsonConvert.DeserializeObject<Product>(redisValue);//反序列化
                    products.Add(product);
                }
                return products;
            }
            #endregion

            #region 3、存储商品对象-集合-分页查询
            {
                // 1、从redis中取对象
                RedisValue[] productvalues = _connectionMultiplexer.GetDatabase(0).SetScan("products", 10, 0, 4).ToArray();
                List<Product> products = new List<Product>();
                if (productvalues.Length == 0)
                {
                    // 2、从数据库中查询
                    products = _productDbContext.Products.ToList();

                    // 3、存储到redis中
                    List<RedisValue> redisValues = new List<RedisValue>();
                    foreach (var product1 in products)
                    {
                        string productjson = JsonConvert.SerializeObject(product1);//序列化
                        redisValues.Add(productjson);
                    }
                    _connectionMultiplexer.GetDatabase(0).SetAdd("products", redisValues.ToArray());
                    return products;
                }

                // 4、序列化，反序列化
                foreach (var redisValue in productvalues)
                {
                    Product product = JsonConvert.DeserializeObject<Product>(redisValue);//反序列化
                    products.Add(product);
                }
                return products;
            }
            #endregion

            // Lua：
            _connectionMultiplexer.GetDatabase(0);
            // ES。模糊搜索。基本内存
        }

        /// <summary>
        /// 扣减商品库存
        /// 做4件事情
        /// </summary>
        /// <returns></returns>
        [HttpGet("SubStock")]
        public IActionResult SubStock()
        {
            #region 1、扣减库存流程
            {

                RedisLock redisLock = new RedisLock();
                redisLock.Lock();
                // 1、获取商品库存
                var stocks = getPorductStocks();

                // 2、判断商品库存是否为空
                if (stocks.Count == 0)
                {
                    // 2.1 秒杀失败消息
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}：不好意思，秒杀已结束，商品编号:{stocks.Count}");
                    redisLock.UnLock();
                    return new JsonResult("秒杀失败");
                }

                // 3、秒杀成功消息
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}：恭喜你，秒杀成功，商品编号:{stocks.Count}");

                // 4、扣减商品库存
                subtracProductStocks(stocks);
                redisLock.UnLock();
                return new JsonResult("秒杀成功");
            }
            #endregion

            #region 2、扣减库存流程-单机并发
            {

            }
            #endregion

            #region 3、扣减库存流程-集群并发
            {
            }
            #endregion

            return new JsonResult("秒杀成功");
        }

        /// <summary>
        /// 获取商品库存
        /// </summary>
        /// <returns></returns>
        private Stocks getPorductStocks()
        {
            // 1、查询数据库获取库存，获取第一个商品的库存数(1)
            Stocks stocks = _productDbContext.Stocks.FirstOrDefault(s => s.Id == 1);

            // 2、返回库存
            return stocks;
        }

        /// <summary>
        /// 扣减商品库存
        /// </summary>
        private void subtracProductStocks(Stocks stocks)
        {
            // 1、扣减商品库存
            Stocks updateStocks = _productDbContext.Stocks.FirstOrDefault(s => s.Id == stocks.Id);
            updateStocks.Count = stocks.Count - 1;

            // 2、更新数据库
            _productDbContext.SaveChanges();
        }
    }
}
