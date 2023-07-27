using LKN.EBusiness.Products;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LKN.EBusiness.Orders
{
    /// <summary>
    /// 订单领域服务
    /// </summary>
    [Dependency(ServiceLifetime.Transient)]
    public class OrderManager : ITransientDependency
    {
        private readonly IProductRepository _productRepository;

        public OrderManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async void CreateAsync(string ProductTitle)
        {
            /*IEnumerable<Product> products = _productRepository.GetProductByName(ProductTitle);
            if (products != null)
            {
                throw new Exception("商品名称不能重复");
            }*/
        }
    }
}
