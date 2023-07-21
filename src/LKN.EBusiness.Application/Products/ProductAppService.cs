using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LKN.EBusiness.Products
{
    public class ProductAppService : CrudAppService<Product, ProductDto, Guid, PagedAndSortedResultRequestDto, CreateProductDto, UpdateProductDto>, IProductAppService
    {
        public IProductAbpRepository _productAbpRepository;

        public ProductAppService(IProductAbpRepository repository) : base(repository)
        {
            _productAbpRepository = repository;
        }

        public IEnumerable<ProductDto> GetProductAndImage()
        {
            // 1、查询所有和图片
            IEnumerable<Product> products = _productAbpRepository.GetProductAndImages();

            // 2、然后映射
            return ObjectMapper.Map<IEnumerable<Product>, List<ProductDto>>(products);
        }

        public IEnumerable<ProductDto> GetProductByAttr(ProductAttrQueryDto createProductDto)
        {
            // 1、查询所有和图片
            IEnumerable<Product> products = _productAbpRepository.GetProductAndImages();

            // 2、然后映射
            return ObjectMapper.Map<IEnumerable<Product>, List<ProductDto>>(products);
        }

        public ProductTotaLDto GetProductTotals()
        {
            throw new NotImplementedException();
        }
    }
}
