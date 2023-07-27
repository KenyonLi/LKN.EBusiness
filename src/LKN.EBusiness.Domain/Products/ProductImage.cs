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
    /// 商品图片
    /// </summary>
    public class ProductImage : FullAuditedEntity<Guid>
    {
        //[Key]
        // public Guid Id { set; get; } // 主键
        public Guid ProductId { set; get; } // 商品编号
        public int ImageSort { set; get; } // 排序
        public string ImageStatus { set; get; } // 状态（1：启用，2：禁用）
        public string ImageUrl { set; get; } // 图片url


        public ProductImage()
        {
        }

        public ProductImage(Guid id) : base(id)
        {

        }
    }
}
