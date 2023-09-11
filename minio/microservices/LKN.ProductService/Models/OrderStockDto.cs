using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YDT.ProductService.Models
{
    /// <summary>
    /// 订单库存 Dto
    /// </summary>
    public class OrderStockDto
    {
        public int ProductId { set; get; }
        public int Stock { set; get; }
    }
}
