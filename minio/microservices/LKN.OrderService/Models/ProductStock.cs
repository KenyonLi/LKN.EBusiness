using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YDT.ProductService.Models
{
    /// <summary>
    /// 商品库存
    /// </summary>
    public class ProductStock
    {
        public int Id { set; get; } // 主键
        public int Stock { set; get; } // 库存
        public int ProductId { set; get; } // 商品Id
    }
}
