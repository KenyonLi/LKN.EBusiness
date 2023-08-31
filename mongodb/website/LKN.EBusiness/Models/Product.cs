using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LKN.EBusiness.Models
{
    public class Product
    {
        public Product() {
            Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { set; get; }
        public string ProductCode { set; get; }    //商品编码
        public string ProductUrl { set; get; }         // 商品主图
        public string ProductTitle { set; get; }       //商品标题
        public string ProductDescription { set; get; }     // 图文描述
        public decimal ProductVirtualprice { set; get; } // 商品虚拟价格
        public decimal ProductPrice { set; get; }       //价格
        public int ProductSort { set; get; }    //商品序号
        public int ProductSold { set; get; }        //已售件数
        public int ProductStock { set; get; }       //商品库存
        public string ProductStatus { set; get; } // 商品状态
        public int score { set; get; } //商品级别
    }
}
