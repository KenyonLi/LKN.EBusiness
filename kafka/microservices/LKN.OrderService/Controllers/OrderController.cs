using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LKN.ProductService.Models;

namespace LKN.OrderService.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    [ApiController]
    [Route("Order")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IDistributedCache distributedCache;

        public OrderController(ILogger<OrderController> logger, 
                                IDistributedCache distributedCache)
        {
            _logger = logger;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Order> OrderCreate()
        {
            #region 1、工作队列(单消费者) Consumer
            {
                /*new Task(() =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        GroupId = "order",
                        EnableAutoCommit = true
                    };
                    var builder = new ConsumerBuilder<string, string>(consumerConfig);

                    using (var consumer = builder.Build())
                    {
                        // 1、订阅
                        consumer.Subscribe("create-order");
                        while (true)
                        {
                            try
                            {
                                // 2、消费(自动确认)
                                var result = consumer.Consume();

                                // 3、业务逻辑:业务逻辑---->执行失败--->消息丢失
                                string key = result.Key;
                                string value = result.Value;

                                _logger.LogInformation($"创建商品：Key:{key}");
                                _logger.LogInformation($"创建商品：Order:{value}");
                            }
                            catch (Exception e)
                            {
                                _logger.LogInformation($"异常：Order:{e}");
                            }
                        }
                    }
                }).Start();*/
            }
            #endregion

            #region 2、工作队列(单消费者)-手动确认消息
            {
                new Task(() =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        GroupId = "order",
                        EnableAutoCommit = false,
                    };
                    var builder = new ConsumerBuilder<string, string>(consumerConfig);
                    var consumer = builder.Build();
                    // 1、订阅
                    consumer.Subscribe("create-order");
                    while (true)
                    {
                        // 2、消费
                        var result = consumer.Consume();

                        // 3、业务逻辑
                        string key = result.Key;
                        string value = result.Value;

                        _logger.LogInformation($"创建商品：Key:{key}");
                        _logger.LogInformation($"创建商品：Order:{value}");

                        // 3、手动提交（向kafka确认消息）----偏移量---消息的序号
                        consumer.Commit(result);
                    }
                }).Start();
            }
            #endregion

            #region 3、工作队列(单消费者)-手动确认消息-偏移量(重复消费)
            {
                /*new Task(() =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        GroupId = "order",
                        EnableAutoCommit = true,
                    };
                    var builder = new ConsumerBuilder<string, string>(consumerConfig);
                    using (var consumer = builder.Build())
                    {
                        // 1、订阅
                        consumer.Subscribe("create-order");
                        // 1.1、重置偏移量
                        consumer.Assign(new TopicPartitionOffset(new TopicPartition("create-order", 0), 18));
                        while (true)
                        {
                            // 2、消费
                            var result = consumer.Consume();

                            // 2.1、获取偏移量
                            _logger.LogInformation($"订单消息偏移量：Offset:{result.Offset}");

                            // 3、业务处理
                            string key = result.Key;
                            string value = result.Value;
                            _logger.LogInformation($"创建订单：Key:{key}");
                            _logger.LogInformation($"创建订单：Order:{value}");

                            // 3、手动提交
                            //consumer.Commit(result);
                        }
                    }
                }).Start();*/
            }
            #endregion

            #region 4、工作队列(单消费者)-手动确认消息-偏移量(重复消费)-存储偏移量
            {
               /* new Task(() =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        GroupId = "order",
                        EnableAutoCommit = true,
                    };
                    var builder = new ConsumerBuilder<string, string>(consumerConfig);
                    using (var consumer = builder.Build())
                    {
                        // 1、订阅
                        consumer.Subscribe("create-order");

                        // 1.2、获取偏移量
                        string offset = distributedCache.GetString("create-order");
                        if (string.IsNullOrEmpty(offset))
                        {
                            offset = "0";
                        }

                        // 1.3、重置偏移量
                        consumer.Assign(new TopicPartitionOffset(new TopicPartition("create-order", 0), int.Parse(offset) + 1));
                        while (true)
                        {
                            // 2、消费
                            var result = consumer.Consume();

                            // 2.1、获取偏移量
                            _logger.LogInformation($"订单消息偏移量：Offset:{result.Offset}");
                            // 2.2、把kafka队列中偏移量存起来。redis mysql
                            // 2.3、重置kafka队列的偏移量
                            distributedCache.SetString("create-order", result.Offset.Value.ToString());

                            // 3、业务处理
                            string key = result.Key;
                            string value = result.Value;
                            _logger.LogInformation($"创建订单：Key:{key}");
                            _logger.LogInformation($"创建订单：Order:{value}");

                            // redis缺陷：无法保证偏移和业务同时成功。
                            // 方案：使用数据库来存储偏移量
                            //       核心：通过数据库事务来保证
                            // 3、手动提交
                           // consumer.Commit(result);
                        }
                    }
                }).Start();*/
            }
            #endregion

            #region 5、订阅发布(广播消费)1、创建订单----2、发送短信-GroupId
            {
                /*new Task(() =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        GroupId = "order",
                        EnableAutoCommit = true,
                    };
                    var builder = new ConsumerBuilder<string, string>(consumerConfig);
                    var consumer = builder.Build();

                    // 1、订阅
                    consumer.Subscribe("create-order");
                    // 2、获取偏移量
                    string offset = distributedCache.GetString("create-order");
                    if (string.IsNullOrEmpty(offset))
                    {
                        offset = "0";
                    }
                    // 3、重置偏移量
                    consumer.Assign(new TopicPartitionOffset(new TopicPartition("create-order", 0), int.Parse(offset)));
                    while (true)
                    {
                        // 2、消费
                        var result = consumer.Consume();
                        // 2.1、获取偏移量
                        _logger.LogInformation($"订单消息偏移量：Offset:{result.Offset}");

                        // 3、业务处理
                        string key = result.Key;
                        string value = result.Value;
                        _logger.LogInformation($"创建商品：Key:{key}");
                        _logger.LogInformation($"创建商品：Order:{value}");

                        // 2.2、把kafka队列中偏移量存起来。redis mysql
                        // 2.3、重置kafka队列的偏移量
                        distributedCache.SetString("create-order", result.Offset.Value.ToString());

                        // 3、手动提交
                        //consumer.Commit(result);
                    }
                }).Start();*/
            }
            #endregion


            #region 6、创建订单----1、创建订单集群或发送短信集群--消息分区
            {
                /*new Task(() =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        GroupId = "order",
                        EnableAutoCommit = false,
                    };
                    var builder = new ConsumerBuilder<string, string>(consumerConfig);
                    var consumer = builder.Build();

                    // 1、订阅
                    consumer.Subscribe("create-order-1");
                    // 2、获取偏移量
                    *//*string offset = distributedCache.GetString("create-order");
                    if (string.IsNullOrEmpty(offset))
                    {
                        offset = "0";
                    }*//*
                    // 3、重置偏移量
                    //consumer.Assign(new TopicPartitionOffset(new TopicPartition("create-order", 1), int.Parse(offset)));
                    while (true)
                    {
                        // 2、消费
                        var result = consumer.Consume();
                        // 2.1、获取偏移量
                        _logger.LogInformation($"订单消息偏移量：Offset:{result.Offset}");
                        // 3、业务处理
                        string key = result.Key;
                        string value = result.Value;
                        _logger.LogInformation($"创建商品：Key:{key}");
                        _logger.LogInformation($"创建商品：Order:{value}");

                        // 2.2、把kafka队列中偏移量存起来。redis mysql
                        // 2.3、重置kafka队列的偏移量
                        //distributedCache.SetString("create-order", result.Offset.Value.ToString());

                        // 3、手动提交
                        consumer.Commit(result);
                    }
                }).Start();*/
            }
            #endregion

            #region 7、创建订单----1、创建订单集群或发送短信集群--消息分区-固定分区
            {
                /*new Task(() =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        GroupId = "order",
                        EnableAutoCommit = false,
                    };
                    var builder = new ConsumerBuilder<string, string>(consumerConfig);
                    var consumer = builder.Build();

                    // 1、订阅
                    consumer.Subscribe("create-order");
                    // 1.1、获取偏移量
                    string offset = distributedCache.GetString("create-order");
                    if (string.IsNullOrEmpty(offset))
                    {
                        offset = "0";
                    }
                    // 1.2、重置偏移量
                    consumer.Assign(new TopicPartitionOffset(new TopicPartition("create-order", 0), int.Parse(offset)));
                    while (true)
                    {
                        // 2、消费
                        var result = consumer.Consume();
                        // 2.1、获取偏移量
                        _logger.LogInformation($"订单消息偏移量：Offset:{result.Offset}");
                        _logger.LogInformation($"订单消息分区序号：Partition:{result.Partition}");

                        // 3、业务处理
                        string key = result.Key;
                        string value = result.Value;
                        _logger.LogInformation($"创建商品：Key:{key}");
                        _logger.LogInformation($"创建商品：Order:{value}");

                        // 2.2、把kafka队列中偏移量存起来。redis mysql
                        // 2.3、重置kafka队列的偏移量
                        distributedCache.SetString("create-order", result.Offset.Value.ToString());

                        // 3、手动提交
                        consumer.Commit(result);
                    }
                }).Start();*/
            }
            #endregion

            // 场景：创建订单，也希望发送短信
            #region 8、创建订单----1、订单消息延迟处理
            {
                //new Task(() =>
                //{
                //    var consumerConfig = new ConsumerConfig
                //    {
                //        BootstrapServers = "127.0.0.1:9092",
                //        AutoOffsetReset = AutoOffsetReset.Earliest,
                //        GroupId = "order",
                //        EnableAutoCommit = false,
                //        FetchMinBytes=170,
                //        FetchMaxBytes=3060
                //    };
                //    var builder = new ConsumerBuilder<string, string>(consumerConfig);
                //    using (var consumer = builder.Build())
                //    {
                //        // 1、订阅
                //        consumer.Subscribe("create-order-1");
                //        // 2、偏移量恢复
                //        string offset = distributedCache.GetString("create-order-1");
                //        if (string.IsNullOrEmpty(offset))
                //        {
                //            offset = "0";
                //        }
                //        consumer.Assign(new TopicPartitionOffset(new TopicPartition("create-order-1", 0), int.Parse(offset)));
                //        while (true)
                //        {
                //            // 1、恢复消息
                //            new Timer((s) =>
                //            {
                //                consumer.Resume(new List<TopicPartition> { new TopicPartition("create-order-1", 0) });
                //            }, null, Timeout.Infinite, Timeout.Infinite).Change(5000, 5000);

                //            // 1.1、消费暂停
                //            consumer.Pause(new List<TopicPartition> { new TopicPartition("create-order-1", 0) });

                //            // 2、消费消息
                //            var result = consumer.Consume(); //批量获取消息，根据-----》字节数
                //            try
                //            {
                //                // 2.1、获取偏移量
                //                _logger.LogInformation($"订单消息偏移量：Offset:{result.Offset}");

                //                // 3、业务处理
                //                string key = result.Key;
                //                string value = result.Value;
                //                _logger.LogInformation($"创建商品：Key:{key}");
                //                _logger.LogInformation($"创建商品：Order:{value}");

                //                // 2.2、把kafka队列中偏移量存起来。redis mysql
                //                // 2.3、重置kafka队列的偏移量
                //                distributedCache.SetString("create-order-1", result.Offset.Value.ToString());

                //                // 3、手动提交
                //                consumer.Commit(result);
                //            }
                //            catch (Exception)
                //            {

                //                throw;
                //            } finally
                //            {
                //                consumer.Pause(new List<TopicPartition> { new TopicPartition("create-order-1", 0) });
                //                Console.WriteLine($"暂停消费");
                //            }
                //        }
                //    }

                //}).Start();
            }
            #endregion

            #region 9、创建订单----1、订单消息延迟处理-批量处理消息
            {
                /*new Task(() =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        GroupId = "order",
                        EnableAutoCommit = false,
                    };
                    var builder = new ConsumerBuilder<string, string>(consumerConfig);
                    using (var consumer = builder.Build())
                    {

                        // 1、订阅
                        consumer.Subscribe("create-order");
                        // 2、偏移量恢复
                        string offset = distributedCache.GetString("create-order");
                        if (string.IsNullOrEmpty(offset))
                        {
                            offset = "0";
                        }
                        consumer.Assign(new TopicPartitionOffset(new TopicPartition("create-order", 0), int.Parse(offset)));
                        while (true)
                        {
                            // 1、恢复消息
                            new Timer((s) =>
                            {
                                consumer.Resume(new List<TopicPartition> { new TopicPartition("create-order", 0) });
                            }, null, Timeout.Infinite, Timeout.Infinite).Change(5000, 5000);

                            // 1.1、消费暂停
                            consumer.Pause(new List<TopicPartition> { new TopicPartition("create-order", 0) });

                            // 2、消费
                            var result = consumer.Consume();
                            // 2.1、获取偏移量
                            _logger.LogInformation($"订单消息偏移量：Offset:{result.Offset}");

                            // 3、业务处理
                            string key = result.Key;
                            string value = result.Value;
                            _logger.LogInformation($"创建商品：Key:{key}");
                            _logger.LogInformation($"创建商品：Order:{value}");

                            // 2.2、把kafka队列中偏移量存起来。redis mysql
                            // 2.3、重置kafka队列的偏移量
                            distributedCache.SetString("create-order", result.Offset.Value.ToString());

                            // 3、手动提交
                            consumer.Commit(result);
                        }
                    }
                }).Start();*/
            }
            #endregion

            Console.WriteLine("订单创建监听......");
            return null;
        }
    }
}
