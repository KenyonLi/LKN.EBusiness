using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 商品服务
    /// </summary>
    public interface IProductAppService:ICrudAppService<ProductDto,Guid,PagedAndSortedResultRequestDto,CreateProductDto,UpdateProductDto>
    {
        public IEnumerable<ProductDto> GetProductAndImage();

        public IEnumerable<ProductDto> GetProductByAttr(ProductAttrQueryDto createProductDto);

        public ProductTotaLDto GetProductTotals();
    }
}
