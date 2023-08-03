using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.Orders
{
    /// <summary>
    /// 创建商品Dto
    /// </summary>
    public class CreateOrderDto
    {

        public decimal OrderTotalPrice { set; get; } // 订单价格

        public int ProductCount { set; get; }// 商品数量

        public CreateOrderItemDto[] OrderItems { set; get; } // 商品图片集合
    }

    public class CreateOrderItemDto
    {
        public CreateOrderItemDto()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { set; get; } // Guid
        public Guid ProductId { set; get; } // 商品编号
        public string ProductName { set; get; } // 商品名称

    }
}