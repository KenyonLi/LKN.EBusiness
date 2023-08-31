using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LKN.EBusiness.Models
{
    /// <summary>
    /// 商品图片
    /// </summary>
    public class ProductImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { set; get; } // 主键
        public int ProductId { set; get; } // 商品编号
        public int ImageSort { set; get; } // 排序
        public string ImageStatus { set; get; } // 状态（1：启用，2：禁用）
        public string ImageUrl { set; get; } // 图片url

    }
}
