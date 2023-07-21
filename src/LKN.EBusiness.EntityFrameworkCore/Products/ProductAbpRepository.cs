using LKN.EBusiness.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 商品仓储实现
    /// </summary>
    [Dependency(ServiceLifetime.Transient)]
    public class ProductAbpRepository : EfCoreRepository<EBusinessDbContext, Product, Guid>, IProductAbpRepository
    {
        public ProductAbpRepository(IDbContextProvider<EBusinessDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }


        public IEnumerable<Product> GetProductAndImages()
        {
            DbSet<Product> products = GetDbSetAsync().Result;
            return products;
        }

        /// <summary>
        /// 根据商品名称 查询
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Product> GetProductByName(string productName)
        {
            //1、第一种实现
            //  EBusinessDbContext db = GetDbContextAsync().Result;
            DbSet<Product> products = base.GetDbSetAsync().Result;
            return products.Where(p=>p.ProductTitle ==productName);
        }
    }
}
