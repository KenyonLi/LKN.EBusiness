using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 商品领域服务
    /// </summary>
    [Dependency(ServiceLifetime.Transient)]
    public class ProductManager /*: DomainService*/
    {
        private readonly IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async void CreateAsync(string ProductTitle)
        {
            Product product1 = _productRepository.GetProductByName(ProductTitle);
            if (product1 != null)
            {
                throw new Exception("商品名称不能重复");
            }
        }
    }
}
