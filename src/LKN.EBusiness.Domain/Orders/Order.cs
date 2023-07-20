using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKN.EBusiness.Orders
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderSn { get; set; }
        /// <summary>
        /// 订单总价
        /// </summary>
        public string OrderTotalPrice { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updatetime { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime Paytime { get; set; }

        public DateTime Sendtime { set; get; }// 发货时间
        public DateTime Successtime { set; get; }// 订单完成时间
        public int OrderStatus { set; get; } // 订单状态
        public string? OrderName { set; get; } // 订单名称
        public string? OrderTel { set; get; } // 订单电话
        public string? OrderAddress { set; get; } // 订单地址
        public string? OrderRemark { set; get; }// 订单备注

        // 订单项
        public List<OrderItem> OrderItems { set; get; }
    }
}
