using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;

namespace LKN.EBusiness.Orders
{ /// <summary>
  /// 订单模型
  /// </summary>
    public class Order : AggregateRoot<Guid>
    {
        //[Key]
        //  public int Id { set; get; } // 主键
        public string OrderType { set; get; } = null; // 订单类型
                                                      // public string OrderFlag { set; get; } // 订单标志
        public Guid? UserId { set; get; } = null; // 用户Id
        public string  OrderSn { set; get; } // 订单号
        public string ? OrderTotalPrice { set; get; } = null; // 订单总价
        public DateTime? Createtime { set; get; } = null; // 创建时间
        public DateTime? Updatetime { set; get; } = null; // 更新时间
        public DateTime? Paytime { set; get; } = null;// 支付时间
        public DateTime? Sendtime { set; get; } = null;// 发货时间
        public DateTime? Successtime { set; get; } // 订单完成时间
        public int? OrderStatus { set; get; }  // 订单状态
        public string? OrderName { set; get; } = null; // 订单名称
        public string? OrderTel { set; get; } = null; // 订单电话
        public string? OrderAddress { set; get; } = null; // 订单地址
        public string? OrderRemark { set; get; } = null;// 订单备注

        // 订单项
        public ICollection<OrderItem> OrderItems { set; get; }


        public Order()
        {
            OrderItems = new Collection<OrderItem>();
        }

        public Order(Guid id) : base(id)
        {
            OrderItems = new Collection<OrderItem>();
        }
        /// <summary>
        /// 更新 订单号
        /// </summary>
        /// <param name="orderSn"></param>
        public void UpOrderItem()
        {
            foreach (var item in OrderItems)
            {
                item.OrderSn = OrderSn;
            }
        }
        /// <summary>
        /// 添加商品图片
        /// </summary>
        public void AddOrderItem(Guid itemId, string OrderSn, string ProductName, decimal ItemPrice)
        {
            // 1、创建一个商品图片
            OrderItem productImage = new OrderItem(itemId);
            productImage.OrderSn = OrderSn;
            productImage.ProductName = ProductName;
            productImage.ItemPrice = ItemPrice;

            // 2、添加到集合中
            OrderItems.Add(productImage);
        }
    }
}
