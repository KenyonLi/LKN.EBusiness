using StackExchange.Redis;

namespace LKN.EBusiness.Locks
{
    /// <summary>
    /// redis分布式锁
    /// 1、封装redis分布锁
    ///    1、加锁
    ///    2、解锁
    /// 2、应用分布式锁
    /// </summary>
    public class ZookeeperLock
    {
        // 1、redis连接管理类
        private ConnectionMultiplexer connectionMultiplexer = null;

        // 2、redis数据操作类
        private IDatabase database = null;
        public ZookeeperLock()
        {
            connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");

            database = connectionMultiplexer.GetDatabase(0);
        }

        /// <summary>
        /// 加锁
        /// 1、key:锁名称
        /// 2、value:谁加的这把锁。线程1
        /// 3、exprie：过期时间：目的是为了防止死锁
        /// </summary>

        public void Lock()
        {
            while (true)
            {
                bool flag = database.LockTake("redis-lock", Thread.CurrentThread.ManagedThreadId, TimeSpan.FromSeconds(10));
                // 1、true ：成功：false 失败
                // 如果加锁失败。
                if (flag)
                {
                    break;
                }

                // 1、怎么防止死循环
                Thread.Sleep(10);
                // 雪花算法
                // 
                // 改造：
                // Guid
                // 
            }
        }

        /// <summary>
        /// 解锁
        /// </summary>

        public void UnLock()
        {
            database.LockRelease("redis-lock", Thread.CurrentThread.ManagedThreadId);

            // 方案：释放资源
            connectionMultiplexer.Close();
        }
    }
}
