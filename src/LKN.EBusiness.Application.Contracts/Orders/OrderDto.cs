using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace LKN.EBusiness.Orders
{
    public class OrderDto : EntityDto<Guid>
    {

        public string OrderType { set; get; } // 订单类型
        public Guid UserId { set; get; } // 用户Id
        public string OrderSn { set; get; }// 订单号
        public string OrderTotalPrice { set; get; } // 订单总价
        public DateTime Createtime { set; get; } // 创建时间
        public DateTime Updatetime { set; get; } // 更新时间
        public DateTime Paytime { set; get; }// 支付时间
        public DateTime Sendtime { set; get; }// 发货时间
        public DateTime Successtime { set; get; }// 订单完成时间
        public int OrderStatus { set; get; } // 订单状态
        public string OrderName { set; get; } // 订单名称
        public string OrderTel { set; get; } // 订单电话
        public string OrderAddress { set; get; } // 订单地址
        public string OrderRemark { set; get; }// 订单备注

        public OrderItemDto[] OrderItems { set; get; }// 订单项集合
    }

    public class OrderItemDto
    {
        public Guid OrderId { set; get; } // 订单编号
        public string OrderSn { set; get; } // 订单号
        public Guid ProductId { set; get; } // 商品编号
        public string ProductUrl { set; get; } // 商品主图
        public string ProductName { set; get; }// 商品名称
        public decimal ItemPrice { set; get; }  // 订单项单价
        public int ItemCount { set; get; } // 订单项数量
        public decimal ItemTotalPrice { set; get; } // 订单项总价
    }


}
