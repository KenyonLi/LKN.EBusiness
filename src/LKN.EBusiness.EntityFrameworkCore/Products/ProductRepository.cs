using LKN.EBusiness.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 商品仓储实现
    /// </summary>
    [Volo.Abp.DependencyInjection.Dependency(ServiceLifetime.Transient)]
    public class ProductRepository : IProductRepository
    {
        public EBusinessDbContext _eBusinessDbContext;
        public ProductRepository(EBusinessDbContext eBusinessDbContext)
        {
            this._eBusinessDbContext = eBusinessDbContext;
        }
        public void Create(Product Product)
        {
            _eBusinessDbContext.Products.Add(Product);
            _eBusinessDbContext.SaveChanges();
        }

        public void Delete(Product Product)
        {
            _eBusinessDbContext.Products.Remove(Product);
            _eBusinessDbContext.SaveChanges();
        }

        public Product GetProductById(Guid id)
        {
            return _eBusinessDbContext.Products.FirstOrDefault(p=>p.Id ==id);
        }

        public Product GetProductByName(string ProductName)
        {
            return _eBusinessDbContext.Products.FirstOrDefault(e => e.ProductTitle == ProductName);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _eBusinessDbContext.Products.ToList();
        }

        public bool ProductExists(Guid id)
        {
            return _eBusinessDbContext.Products.Any(e => e.Id == id);
        }

        public void Update(Product Product)
        {
            _eBusinessDbContext.Products.Update(Product);
            _eBusinessDbContext.SaveChanges();
        }
    }
}
