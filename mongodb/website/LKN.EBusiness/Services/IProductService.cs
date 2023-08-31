using LKN.EBusiness.Models;

namespace LKN.EBusiness.Services
{
    /// <summary>
    /// 商品服务接口
    /// </summary>
    public interface IProductService
    {
        void Create(Product Product);
        public void CreateList(List<Product> Products);

        IEnumerable<Product> GetProducts();
        Product GetProductById(string id);
        public IEnumerable<Product> GetProductsByPage(int Page, int PageSize);
        public IEnumerable<Product> GetProductsBySort(Product product, int Page, int PageSize);
        public IEnumerable<int> GetProductsByAggregation(Product product);
        void Update(string id, Product Product);
        public void UpdateList(string id, Product Product);
        // 更新字段
        public void UpdateFiled(string id, ProductUpdateFiledDto productUpdateFiledDto);
        void Delete(Product Product);
        public void DeleteList(Product Product);
        bool ProductExists(string id);

        // 字段修改
        public void Replace(string id, Product Product);

        // 创建索引
        public string CreateIndex();
    }
}
