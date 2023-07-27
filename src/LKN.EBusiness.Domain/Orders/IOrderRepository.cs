using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LKN.EBusiness.Orders
{
    /// <summary>
    /// 订单仓储
    /// 1、做定制化
    /// </summary>
    public interface IOrderRepository:IRepository<Order, Guid>
    {
    }
}
