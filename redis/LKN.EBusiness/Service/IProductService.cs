using LKN.EBusiness.Contexts;
using LKN.EBusiness.Locks;
using LKN.EBusiness.Models;
using Microsoft.EntityFrameworkCore;

namespace LKN.EBusiness.Service
{
     /// <summary>
      /// 商品服务
      /// </summary>
        public class IProductService
        {
            private readonly ProductDbContext _productDbContext;
            public IProductService(ProductDbContext productDbContext)
            {
                _productDbContext = productDbContext;
            }

            public void SubStock()
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
                    return;
                }

                // 3、秒杀成功消息
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}：恭喜你，秒杀成功，商品编号:{stocks.Count}");

                // 4、扣减商品库存
                subtracProductStocks(stocks);
                redisLock.UnLock();
            }


            /// <summary>
            /// 获取商品库存
            /// </summary>
            /// <returns></returns>
            private Stocks getPorductStocks()
            {
                // 1、查询数据库获取库存，获取第一个商品的库存数(1)
                var stocksee = _productDbContext.Stocks.Where(s => s.Id == 1).AsNoTracking();
                foreach (var stock in stocksee)
                {
                    return stock;
                }

                return null;
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
