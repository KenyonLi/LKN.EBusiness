using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKN.ProductService.Models
{
    /// <summary>
    /// 商品创建Dto
    /// </summary>
    public class ProductCreateDto
    {
        public int ProductName { set; get; }
        public int ProductPrice { set; get; }
    }
}
