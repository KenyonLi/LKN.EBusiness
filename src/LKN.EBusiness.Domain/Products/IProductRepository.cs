using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 商品仓储
    /// </summary>
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(Guid id);

        Product GetProductByName(string ProductName);
        void Create(Product Product);
        void Update(Product Product);
        void Delete(Product Product);
        bool ProductExists(Guid id);
    }
}
