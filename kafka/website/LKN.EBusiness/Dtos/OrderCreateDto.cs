using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKN.ProductService.Models
{
    /// <summary>
    /// 订单创建Dto
    /// </summary>
    public class OrderCreateDto
    {
        public int ProductName { set; get; }
        public int ProductPrice { set; get; }
    }
}
