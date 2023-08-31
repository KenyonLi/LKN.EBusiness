using LKN.EBusiness.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LKN.EBusiness.Services
{
    /// <summary>
    /// 商品服务实现
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(
            /* IMongoCollection<Product> products,*/
            IConfiguration configuration/*,IOptions<ProductMongoDBOptions> options*/)
        {
            // 1、建立MongoDB连接

          ///  var client = new MongoClient("mongodb://localhost:27018,localhost:27019,localhost:27020");
          //连接分片集群（连接路由就可以了）
            var client = new MongoClient("mongodb://localhost:27000,localhost:27100");
           // var client = new MongoClient("mongodb://localhost:27017");
            // 2、获取商品库(自己创建商品数据)
            var database = client.GetDatabase("productshard");

            // 3、获取商品表(自己创建商品数)
            _products = database.GetCollection<Product>("Product");
            // _products = products;
        }

        public void Create(Product Product)
        {
            _products.InsertOne(Product);
        }

        public void CreateList(List<Product> Products)
        {
            _products.InsertMany(Products);
        }

        public void Delete(Product Product)
        {
            _products.DeleteOne(product => product.Id == Product.Id);
        }

        public void DeleteList(Product Product)
        {
            _products.DeleteMany(product => product.Id == Product.Id);
        }

        public Product GetProductById(string id)
        {
            return _products.Find<Product>(product => product.Id == id).FirstOrDefault();
        }

        public IEnumerable<Product> GetProducts()
        {
            return _products.Find(product => true).ToList();
        }

        /// <summary>
        /// 商品不同字段获取
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetProductsFiled()
        {
            var filter = Builders<BsonDocument>.Filter;
            /* BsonDocument filter = new BsonDocument();
             var collection = _mongoDatabase.GetCollection<BsonDocument>("ProductDb");
             var s = collection.Find(filter).ToList();
             using (var cursor = collection.FindAsync(filter).Result)
             {
                 while (cursor.MoveNextAsync().Result)
                 {
                     var batch = cursor.Current;
                     foreach (BsonDocument document in batch)
                     {
                         Console.WriteLine(document.ToJson());
                     }
                 }
             }*/
            return _products.Find(product => true).ToList();
        }

        /*/// <summary>
        /// 商品数量查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Product"></param>
        public long GetCount(int Page, int PageSize)
        {
            var skip = (Page - 1) * PageSize;
            return _products.Count();
        }*/

        /// <summary>
        /// 商品分页查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Product"></param>
        public IEnumerable<Product> GetProductsByPage(int Page, int PageSize)
        {
            var skip = (Page - 1) * PageSize;
            return _products.Find(x => true).Skip(skip).Limit(PageSize).ToList();
        }

        /// <summary>
        /// 商品排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Product"></param>
        public IEnumerable<Product> GetProductsBySort(Product product, int Page, int PageSize)
        {
            var skip = (Page - 1) * PageSize;
            return _products.Find(product => true).SortBy(product => product.ProductSort).ToList();
        }

        /// <summary>
        /// 商品价格聚合查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Product"></param>
        public IEnumerable<int> GetProductsByAggregation(Product product)
        {
            var filter = Builders<BsonDocument>.Filter;
            PipelineDefinition<Product, int> pipelineDefinitions = PipelineDefinition<Product, int>.Create("ProductPrice");
            var ints = _products.Aggregate<int>(pipelineDefinitions);
            while (ints.MoveNext())
            {
                var test = ints.Current;
                return test;
            }
            return null;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Product"></param>
        public void Update(string id, Product Product)
        {
            var update = Builders<Product>.Update;
            _products.UpdateOne(product => product.Id == id, update.Set("ProductTitle", Product.ProductTitle));
        }

        /// <summary>
        /// 更新字段(增加字段)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Product"></param>
        public void UpdateFiled(string id, ProductUpdateFiledDto productUpdateFiledDto)
        {
            var update = Builders<Product>.Update;
            _products.UpdateOne(product => product.Id == id, update.AddToSet("ProductTest", productUpdateFiledDto.ProductLike));
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Product"></param>
        public void UpdateList(string id, Product Product)
        {
            var filter = Builders<Product>.Filter;
            var update = Builders<Product>.Update;
            _products.UpdateMany(product => product.ProductCode == Product.ProductCode, update.Set("ProductTitle", Product.ProductTitle));
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        public string CreateIndex()
        {
            var indexKeys = Builders<Product>.IndexKeys;
            return _products.Indexes.CreateOne(indexKeys.Descending("ProductCode"));
        }


        // 原理
        // 分片集群
        // 分片复制集群

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Product"></param>
        public void Replace(string id, Product Product)
        {
            _products.ReplaceOneAsync(x => x.Id == id, Product);
        }

        public bool ProductExists(string id)
        {
            Product product = _products.Find<Product>(product => product.Id == id).FirstOrDefault();
            if (product != null)
            {
                return true;
            }
            return false;
        }
    }
}
