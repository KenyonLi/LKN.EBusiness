using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 禁止 ABP 默认的生成的API接口，创建、添加、查询
    /// </summary>
    //[RemoteService(IsEnabled = false)]

    public class ProductAppService : CrudAppService<Product, ProductDto, Guid, PagedAndSortedResultRequestDto, CreateProductDto, UpdateProductDto>, IProductAppService
    {
        public IProductAbpRepository _productAbpRepository;

        public ProductAppService(IProductAbpRepository repository) : base(repository)
        {
            _productAbpRepository = repository;
        }
        [RemoteService(IsEnabled = false)]
        public IEnumerable<ProductDto> GetProductAndImage()
        {
            // 1、查询所有和图片
            IEnumerable<Product> products = _productAbpRepository.GetProductAndImages();

            // 2、然后映射
            return ObjectMapper.Map<IEnumerable<Product>, List<ProductDto>>(products);
        }
        [RemoteService(IsEnabled = false)]
        public IEnumerable<ProductDto> GetProductByAttr(ProductAttrQueryDto createProductDto)
        {
            // 1、查询所有和图片
            IEnumerable<Product> products = _productAbpRepository.GetProductByName(createProductDto.productName);

            // 2、然后映射
            return ObjectMapper.Map<IEnumerable<Product>, List<ProductDto>>(products);
        }

        public ProductTotaLDto GetProductTotals()
        {
            throw new NotImplementedException();
        }
    }
}
