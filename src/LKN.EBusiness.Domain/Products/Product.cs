using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LKN.EBusiness.Products
{

    /// <summary>
    /// 商品模型
    /// 1、int类型主键
    ///     优势：查询性能比较快
    ///     缺点：不能在集群数据库中做唯一主键
    ///            原因：本身自增性导致
    ///     使用时机：单库
    ///     
    /// 2、Guid 类型主键
    ///      优点：分布式环境保证数据唯一
    ///      缺点：查询性能稍微比int类型性能低一点
    ///      使用时机：分布式数据库
    ///      
    /// 实现BasicAggregateRoot只管理Id
    /// 
    /// 第一个方面优化:AggregateRoot<Guid> Entity<Guid>
    /// 
    /// 第二个方面优化：实体属性复用优化（复用字段）
    /// 
    /// 第三个知识点：扩展方面的知识点
    /// 总结：
    /// 1、领域层的ABP框架优化
    ///    1.1、聚合根和实体
    ///    1.2、审计属性
    ///    1.3、每一个实体对应的流水表
    ///    1.4、涉及到大数据的概念
    /// 源码模块
    /// Volo.Abp.Auditing
    /// Volo.Abp.Data
    /// Volo.Abp.Ddd.Domain
    /// 
    /// 
    /// 1、删除优化
    /// 
    /// 2、增删除改查时间和人
    ///
    /// </summary>
    public class Product : FullAuditedAggregateRoot<Guid>,IMultiTenant
    {
        // public Guid Id { set; get; } // 商品主键
        public string? ProductCode { set; get; }    //商品编码
        public string? ProductUrl { set; get; }         // 商品主图
        public string? ProductTitle { set; get; }       //商品标题
        public string? ProductDescription { set; get; }     // 图文描述
        public decimal? ProductVirtualprice { set; get; } //商品虚拟价格
        public decimal? ProductPrice { set; get; }       //价格
        public int? ProductSort { set; get; }    //商品序号
        public int? ProductSold { set; get; }        //已售件数
        public int? ProductStock { set; get; }       //商品库存
        public string? ProductStatus { set; get; } // 商品状态


        // 接口规范两种类型
        // 1、对属性规范。属性复用
        // 2、方法规范。

        /*public string VirturePrice { set; get; } // 虚拟价格
        public string ProductSort { set; get; } // 商品排序*/

        // 随意增加字段导致结果
        // 1、实体违背开闭原则
        // 2、导致代码不稳定

        // ExtraProperties :动态适应客户需求。动态属性
        // 使用json来存储字段信息
        // 例如：{VirturePrice:1,ProductSort:2}
        // 本质：就是一个字典。Dictionary

        // ConcurrencyStamp
        // 隔离线程操作：保证数据只有一个线程在处理
        // 乐观锁的方式。

        public virtual ICollection<ProductImage> ProductImages { get; set; }

        public Guid? TenantId { get; set; }

        public Product()
        {
            ProductImages = new Collection<ProductImage>();
        }

        public Product(Guid id) : base(id)
        {
            ProductImages = new Collection<ProductImage>();
        }

        public Product(string ProductTitle)
        {
            // 业务规则：ProductTitle名称不能为空
            if (ProductTitle == null)
            {
                throw new Exception("商品名称不能为空");
            }
            ProductImages = new Collection<ProductImage>();
        }

        /// <summary>
        /// 添加商品图片
        /// </summary>
        public void AddProductImage(Guid ImageId, string ImageUrl)
        {
            // 1、创建一个商品图片
            ProductImage productImage = new ProductImage(ImageId);
            productImage.ImageUrl = ImageUrl;

            // 2、添加到集合中
            ProductImages.Add(productImage);
        }


        /// <summary>
        /// 添加商品图片
        /// </summary>
        public void RemoveProductImage(Guid productImageId)
        {
            // 1、判断guid ,然后删除指定商品


            // 2、添加到集合中
            // ProductImages.Remove(productImage);
        }
    }
}
