using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LKN.EBusiness.Orders
{
    /// <summary>
    /// 商品应用服务接口
    /// </summary>
    public  interface IOrderAppService: ICrudAppService<OrderDto,Guid,PagedAndSortedResultRequestDto,CreateOrderDto,UpdateOrderDto>
    {
        
    }
}
