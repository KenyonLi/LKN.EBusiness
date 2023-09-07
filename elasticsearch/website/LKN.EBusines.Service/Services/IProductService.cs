using LKN.EBusines.Service.Models;
using Nest;

namespace LKN.EBusines.Service.Services
{
    /// <summary>
    /// 商品服务接口
    /// </summary>
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProductsByPage(int page, int pageSize);
        IEnumerable<Product> GetProducts(List<string> ids);

        /// <summary>
        /// 全文搜索(keyword查询)
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetProductsKeywordSearch(ProductDto productDto);

        /// <summary>
        /// 全文搜索(text查询)
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetProductsTextSearch(ProductDto productDto);

        /// <summary>
        /// 全文搜索(text范围查询)
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetProductsRangeTextSearch(ProductDto productDto);

        /// <summary>
        /// 全文搜索(text聚合查询)
        /// </summary>
        /// <returns></returns>
        ValueAggregate GetProductsAggreateTextSearch(ProductDto productDto);

        /// <summary>
        /// 全文搜索(text查询)---（排序）
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetProductsTextSearchSort(ProductDto productDto);

        /// <summary>
        /// 全文搜索(MutilText查询)
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetProductsMutilTextSearch(ProductDto productDto);

        /// <summary>
        /// 全文搜索(嵌套文档查询)
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetProductsNestedTextSearch(ProductDto productDto);


        /// <summary>
        /// 全文搜索(扩展(地理位置查询和IP查询))
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetProductsIPTextSearch(ProductDto productDto);

        Product GetProductById(string id);
        void Create(Product Product);
        void CreateList(List<Product> Products);
        void Update(Product Product);
        void UpdateList(List<Product> Products);
        void Delete(string Id);
        void Deletelist(List<Product> Products);
        bool ProductExists(int id);
    }
}
