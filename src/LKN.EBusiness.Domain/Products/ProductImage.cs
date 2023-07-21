using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace LKN.EBusiness.Products
{

    /// <summary>
    /// 聚合对象
    /// </summary>
    public class ProductImage: FullAuditedEntity<Guid>
    {
       // public Guid Id { get; set; }
        /// <summary>
        /// 商品编号 
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int ImageSort { get; set; }

        /// <summary>
        /// 状态 1：启用 2 禁用
        /// </summary>
        public  string ImageStatus { get; set; } 

        /// <summary>
        /// 图片url
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
