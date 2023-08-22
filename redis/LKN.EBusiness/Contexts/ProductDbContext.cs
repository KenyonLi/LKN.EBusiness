using LKN.EBusiness.Models;
using Microsoft.EntityFrameworkCore;

namespace LKN.EBusiness.Contexts
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Stocks> Stocks { get; set; }
        public DbSet<Product> Products { set; get; }
    }
}
