using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 聚合根 来管理所有的聚合对象
    /// </summary>
    public class Product
    {
        public Guid Id { get; set; }         
        /// <summary>
        /// 商品编码
        /// </summary>
        public string? ProductCode { get; set; }
        /// <summary>
        /// 商品主图
        /// </summary>
        public string ProductUrl { get; set; }
        /// <summary>
        /// 商品标题
        /// </summary>
        public string ProductTitle { get; set; }
        /// <summary>
        /// 图文描述
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// 商品虚拟价格
        /// </summary>
        public decimal ProductVirtualprice { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal ProductPrice { get; set; }
        /// <summary>
        /// 商品序号
        /// </summary>
        public int ProductSort { get; set; }
        /// <summary>
        //已售件数
        /// </summary>
        public int ProductSold { get; set; }

        /// <summary>
        /// 商品库存
        /// </summary>
        public int ProductStock { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        public string ProductStatus { get; set; }


        public virtual ICollection<ProductImage> ProductImages { get; set; }


        public Product() { 
        
           ProductImages = new HashSet<ProductImage>();
        }
    }
}
