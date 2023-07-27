using LKN.EBusiness.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LKN.EBusiness.Orders
{
    /// <summary>
    /// 商品仓储实现
    /// </summary>
    [Dependency(ServiceLifetime.Transient)]
    public class OrderRepository : EfCoreRepository<EBusinessDbContext, Order, Guid>, IOrderRepository
    {
        public OrderRepository(
            IDbContextProvider<EBusinessDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

    }
}
