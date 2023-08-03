using Castle.Core.Smtp;
using LKN.EBusiness.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LKN.EBusiness.Orders
{
    /// <summary>
    /// 订单服务实现
    /// </summary>
    public class OrdderAppService : EBusinessAppService, IOrderAppService
    {
        public IOrderRepository _orderRepository { get; set; }
        /// <summary>
        /// 商品仓储接口
        /// </summary>
        public IProductAbpRepository _productAbpRepository { get; set; }

        public IEmailSender _emailSender { get; set; } // 邮件依赖
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {

            var guid = CurrentTenant.Id;
            //1、创建订单
            Order order = new Order(GuidGenerator.Create());
            order = ObjectMapper.Map<CreateOrderDto, Order>(input, order);
            order.OrderSn = Guid.NewGuid().ToString();
            order.UpOrderItem();

            //1、获取用户信息
            //Claim[] claims = CurrentUser.GetAllClaims();
            // 2. 保存订单
            await _orderRepository.InsertAsync(order);
            // 3、扣减商品库存
            // 1、先查询商品
            /*foreach (var orderItemDto in input.OrderItems)
            {
                Product product = await _productAbpRepository.GetAsync(orderItemDto.ProductId);
                product.ProductStock = product.ProductStock - input.ProductCount;
                await _productAbpRepository.UpdateAsync(product);
            }

            await CurrentUnitOfWork.SaveChangesAsync();*/

            // 3.1 发送邮件
           // string flag = FeatureChecker.GetOrNullAsync(EBusinessFeatures.Orders.IsEmail).Result;
           // if (flag.Equals("true"))
           // {
           //     Console.WriteLine("发送邮件");
           // }

           // // 3.2 发送短信
           //// string IsSmsflag = FeatureChecker.GetOrNullAsync(EBusinessFeatures.Orders.IsSms).Result;
           // if (IsSmsflag.Equals("true"))
           // {
           //     Console.WriteLine("发送短信");
           // }

            // 权限，特征，设置。操作一模一样。本质：都是字符串。
            // 前提(场景)不一样。
            // 权限：用户 功能多少
            // 特征：多租户 。功能的多少。多租户功能多少
            // 设置：系统配置。功能的种类（选择）。多少不变，种类改变

            // 特征：默认是根据多租户来决定多少功能？
            // 扩展：根据用户来决定多少功能

            // _emailSender.SendAsync("网站","QQ","订单创建成功");

            // 4、返回订单
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> UpdateAsync(Guid id, UpdateOrderDto input)
        {
            throw new NotImplementedException();
        }
    }
}
