using Elasticsearch.Net;
using LKN.EBusines.Service.Models;
using Microsoft.Extensions.Options;
using Nest;

namespace LKN.EBusines.Service.Services
{
    /// <summary>
    /// 商品服务实现
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ElasticClient elasticClient;

        public ProductService(/*IConfiguration configuration*/IOptions<ProductMongoDBOptions> options)
        {
            /* ProductMongoDBOptions productMongoDBOptions = options.Value;
             // 1、建立MongoDB连接
             var client = new MongoClient(productMongoDBOptions.ConnectionString);

             // 2、获取商品库
             var database = client.GetDatabase("productdb");

             // 3、获取商品表(集合)
             _products = database.GetCollection<Product>("Product");*/
            #region 1、单实例连接
            {
                var node = new Uri("http://localhost:9200");
                // var defaultIndex = "products";

                var settings = new ConnectionSettings(node);
                //.DefaultIndex(defaultIndex);

                elasticClient = new ElasticClient(settings);
            }
            #endregion

            #region 2、集群连接
            //{
            //    var nodes = new Uri[]
            //    {
            //        new Uri("http://localhost:9201"),
            //        new Uri("http://localhost:9202"),
            //        new Uri("http://localhost:9203"),
            //    };
            //    var pool = new StaticConnectionPool(nodes);
            //    var settings = new ConnectionSettings(pool);

            //    elasticClient = new ElasticClient(settings);
            //}
            #endregion

        }

        public void Create(Product Product)
        {
            elasticClient.Index(Product, idx => idx.Index("products")); // 创建数据库
        }

        public void CreateList(List<Product> Products)
        {
            elasticClient.IndexMany<Product>(Products, "products");
        }

        public void Delete(string Id)
        {
            elasticClient.Delete<Product>(Id, idx => idx.Index("products"));
        }

        public void Deletelist(List<Product> Products)
        {
            elasticClient.DeleteMany<Product>(Products, "products");
        }

        public Product GetProductById(string id)
        {
            return elasticClient.Get<Product>(id, idx => idx.Index("products")).Source;
        }

        public IEnumerable<Product> GetProducts()
        {
            #region 1、直接查询
            {
                var request = new SearchRequest("products");
                var dems = elasticClient.Search<Product>(request);
                return dems.Documents;
            }
            #endregion

            #region 2、委托查询
            {
                return elasticClient.Search<Product>(s => s
                .Index("products")
                ).Documents;
            }
            #endregion
        }

        public IEnumerable<Product> GetProductsByPage(int page, int pageSize)
        {
            #region 1、直接查询
            {
                var request = new SearchRequest("products");
                request.From = (page - 1) * pageSize;
                request.Size = pageSize;

                return elasticClient.Search<Product>(request).Documents;
            }
            #endregion

            #region 2、委托查询
            {
                return elasticClient.Search<Product>(s => s
                .Index("products")
                .From((page - 1) * pageSize)
                .Size(pageSize)
                ).Documents;
            }
            #endregion
        }

        public IEnumerable<Product> GetProducts(List<string> ids)
        {
            List<Product> products = new List<Product>();
            var response = elasticClient.GetMany<Product>(ids, "products");

            foreach (var multiGetHit in response)
            {
                if (multiGetHit.Found)
                {
                    products.Add(multiGetHit.Source);
                }
            }
            return products;
        }

        public IEnumerable<Product> GetProductsKeywordSearch(ProductDto productDto)
        {
            return elasticClient.Search<Product>(s => s
             .Index("products_cluster")
             .Query(q => q
               .Match(mq => mq.Field(f => f.ProductTitle).Query(productDto.ProductTitle))
             )
            ).Documents;
        }

        public IEnumerable<Product> GetProductsTextSearch(ProductDto productDto)
        {
            #region 1、单文本查询
            {
                return elasticClient.Search<Product>(s => s
                .Index("products")
                .Query(q => q
                  .Term(t => t.ProductTitle, productDto.ProductTitle)
                )
               ).Documents;
            }
            #endregion
        }

        public IEnumerable<Product> GetProductsRangeTextSearch(ProductDto productDto)
        {
            #region 1、单文本范围查询
            {
                return elasticClient.Search<Product>(s => s
                .Index("products")
                .Query(q => q
                   .TermRange(tr => tr.Field(f => f.ProductPrice).GreaterThanOrEquals("19.00").LessThan("100.00"))
                )
               ).Documents;
            }
            #endregion
        }

        public ValueAggregate GetProductsAggreateTextSearch(ProductDto productDto)
        {
            #region 1、聚合查询(平均值)
            {
                //var ducmentsss = elasticClient.Search<Product>(s => s
                //         .Index("products")
                //         .Query(q => q.Match(mq => mq.Field(f => f.ProductTitle).Query(productDto.ProductTitle)))
                //         .Aggregations(a => a.Average("ProductPrice_Average", aa => aa.Field(f => f.ProductPrice)))
                //         .Aggregations(a => a.Min("ProductPrice_Min", aa => aa.Field(f => f.ProductPrice)))
                //         .Aggregations(a => a.Max("ProductPrice_Max", aa => aa.Field(f => f.ProductPrice)))
                //         .Aggregations(a => a.Sum("ProductPrice_Sum", aa => aa.Field(f => f.ProductPrice)))
                //       ).Aggregations.Min("ProductPrice_Max");

                //return ducmentsss;

                var ducmentsss = elasticClient.Search<Product>(s => s
                         .Index("products")
                         .Query(q => q.Match(mq => mq.Field(f => f.ProductTitle).Query(productDto.ProductTitle)))
                         .Aggregations(a => a.Average("ProductPrice_Average", aa => aa.Field(f => f.ProductPrice)))
                       ).Aggregations.Average("ProductPrice_Average");
                return ducmentsss;
            }
            #endregion

            #region 2、聚合查询(求和)
            {
                /*return elasticClient.Search<Product>(s => s
                        .Index("products")
                        .Query(q => q
                           .Term(t => t.ProductTitle, productDto.ProductTitle)
                        ).Aggregations(a => a
                           .Sum("ProductPrice_Sum", aa => aa.Field(f => f.ProductPrice))
                        )
                       ).Aggregations.Sum("ProductPrice_Sum");*/
            }
            #endregion

            #region 3、聚合查询(最小值)
            {
                /*return elasticClient.Search<Product>(s => s
                        .Index("products")
                        .Query(q => q
                           .Term(t => t.ProductTitle, productDto.ProductTitle)
                        ).Aggregations(a => a
                           .Min("ProductPrice_Min", aa => aa.Field(f => f.ProductPrice))
                        )
                       ).Aggregations.Min("ProductPrice_Min");*/
            }
            #endregion

            #region 4、聚合查询(最大值)
            {
                /*return elasticClient.Search<Product>(s => s
                        .Index("products")
                        .Query(q => q
                           .Term(t => t.ProductTitle, productDto.ProductTitle)
                        ).Aggregations(a => a
                           .Max("ProductPrice_Max", aa => aa.Field(f => f.ProductPrice))
                        )
                       ).Aggregations.Max("ProductPrice_Max");*/
            }
            #endregion

            #region 5、聚合查询(百分数)
            {
                var test = elasticClient.Search<Product>(s => s
                        .Index("products")
                        .Query(q => q
                           .Term(t => t.ProductTitle, productDto.ProductTitle)
                        ).Aggregations(a => a
                           .Percentiles("ProductPrice_Percentiles", aa => aa.Field(f => f.ProductPrice))
                        )
                       ).Aggregations.Percentiles("ProductPrice_Percentiles");

                return null;
            }
            #endregion
        }

        public IEnumerable<Product> GetProductsTextSearchSort(ProductDto productDto)
        {
            #region 1、文本查询(升序排序)
            {
                return elasticClient.Search<Product>(s => s
                        .Index("products")
                        .Query(q => q
                           .Term(t => t.ProductTitle, productDto.ProductTitle)
                        ).Sort(s => s.Ascending(a => a.Id))
                       ).Documents;
            }
            #endregion

            #region 1、文本查询(降序序排序)
            {
                return elasticClient.Search<Product>(s => s
                        .Index("products")
                        .Query(q => q
                           .Term(t => t.ProductTitle, productDto.ProductTitle)
                        ).Sort(s => s.Descending(a => a.Id))
                       ).Documents;
            }
            #endregion
        }

        public IEnumerable<Product> GetProductsMutilTextSearch(ProductDto productDto)
        {
            #region 1、多文本 ||查询
            {
                return elasticClient.Search<Product>(s => s
              .Index("products")
              .Query(q => q
                .Term(t => t.ProductTitle, "tony") || q
                .Term(t => t.ProductDescription, "text")
              )
             ).Documents;
            }
            #endregion

            #region 2、多文本 && 查询
            {
                return elasticClient.Search<Product>(s => s
                  .Index("products")
                  .Query(q => q
                    .Term(t => t.ProductTitle, "tony") && q
                    .Term(t => t.ProductDescription, "text")
                  )
                 ).Documents;
            }
            #endregion
        }


        public IEnumerable<Product> GetProductsNestedTextSearch(ProductDto productDto)
        {
            #region 1、对象嵌套查询
            {
                return elasticClient.Search<Product>(s => s
                  .Index("products")
                  .Query(q => q
                   //.Term(t => t.ProductDescription, "tony")
                   /*.Nested(n => n
                     .Path(p => p.productSales)
                     .Query(q => q
                          .Term(m => m
                             .Field( f => f.productSales.SoldCount)
                             .Value("2")
                          )
                       )
                    )*/
                   )).Documents;
            }
            #endregion

            #region 2、数组嵌套查询
            {
                return elasticClient.Search<Product>(s => s
                  .Index("products")
                  .Query(q => q
                   //.Term(t => t.ProductDescription, "tony")
                   /*.Nested(n => n
                     .Path( p => p.productImages)
                     .Query( q => q
                          .Match( m => m
                             .Field(f => f.productImages[0].ImageStatus)
                             .Query("1")
                           )
                      )
                    )*/
                   )).Documents;
            }
            #endregion
        }

        public void Update(Product Product)
        {
            elasticClient.Update<Product>(Product.Id, idx =>
                idx.Upsert(Product).Index("products")
            );
        }

        public void UpdateList(List<Product> Products)
        {
            // elasticClient.PutScript
            // elasticClient.SearchShards
            // elasticClient.UpdateByQuery
        }

        public bool ProductExists(int id)
        {
            return elasticClient.DocumentExists<Product>(id, idx => idx.Index("products")).Exists;
        }

        public IEnumerable<Product> GetProductsIPTextSearch(ProductDto productDto)
        {
            throw new NotImplementedException();
        }
    }
}