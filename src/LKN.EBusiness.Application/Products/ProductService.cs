using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace LKN.EBusiness.Products
{
    /// <summary>
    /// 商品服务实现
    /// </summary>
    [Dependency(ServiceLifetime.Transient)]
    public class ProductService : IProductService
    {
        public IProductRepository _productRepository; // 商品仓储

        public ProductManager _ProductManager; // 商品领域服务
        protected IGuidGenerator GuidGenerator { get; set; } // guid 生成器
        protected IObjectMapper ObjectMapper { get; set; }

        public ProductService(IProductRepository ProductRepository, ProductManager productManager)
        {
            this._productRepository = ProductRepository;
            this._ProductManager = productManager;
        }

        /// <summary>
        ///  需求：1、只添加商品
        ///  需求：2、添加商品时，同时添加商品图片
        ///  需求：3、已经添加了商品。如果再次添加图片。
        ///  
        ///  是一种，普遍的需求。所有的类似操作，都可以按照这三个需求的思路来实现。
        ///  例如：订单领域
        ///        消费者领域
        ///   
        /// 添加了，对于
        /// 1、查询
        ///    需求：1、只查询商品
        ///    需求：2、查询商品，又查询商品图片
        /// 2、删除
        ///    需求：1、删除商品，同时删除商品图片
        ///    需求：2、不删除商品，但是只删除图片
        /// 3、修改
        ///    需求：1、只修改商品
        ///    需求：2、已经存在的商品，修改商品图片
        ///    需求：3、即修改商品，又修改商品图片
        ///    
        /// 规则：（是人为想象的）
        /// 增加商品规则
        ///   规则1：添加商品，名称不能重复
        ///         如何实现。
        /// 删除商品规则
        /// 修改商品规则
        /// 查询商品规则
        /// 
        /// 总结
        /// 业务逻辑：数据+规则（商品+if判断）
        /// 
        /// 如何判断是否是应用层逻辑，还是领域层业务逻辑
        /// 总结：根据逻辑是否复用而定。
        /// </summary>
        /// <param name="createProductDto"></param>
        public void Create(CreateProductDto createProductDto)
        {
            // 1、AutoMapper自动映射实体
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateProductDto, Product>();
                cfg.CreateMap<ProductImageCreateDto, ProductImage>();
            });

            IMapper mapper = configuration.CreateMapper();
            GuidGenerator.Create();
            Product product = mapper.Map<CreateProductDto, Product>(createProductDto);

            // 1、先查询商品
            /*Product product1 = _productRepository.GetProductByName(ProductTitle);
            if (product1 != null)
            {
                throw new Exception("商品名称不能重复");
            }*/
            // 1、规则判断
            _ProductManager.CreateAsync(product.ProductTitle);

            // 2、创建商品
            _productRepository.Create(product);
        }

        public void CreateProductImage(Guid ProductId, ProductImageCreateDto productImageCreateDto)
        {
            throw new NotImplementedException();
        }

        public void Delete(DeleteProductDto deleteProductDto)
        {
            throw new NotImplementedException();
        }

        public void DeleteProductImage(Guid ProductId, DeleteProductImageDto deleteProductImageDto)
        {
            throw new NotImplementedException();
        }

        public ProductDto GetProductById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void GetProductByName()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductDto> GetProducts()
        {

            // 1、数据库查询数据
            IEnumerable<Product> products = _productRepository.GetProducts();
            // 2、AutoMapper自动映射实体
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
            });

            IMapper mapper = configuration.CreateMapper();

            List<ProductDto> productDtos = mapper.Map<IEnumerable<Product>, List<ProductDto>>(products);
            return productDtos;
        }

        public bool ProductExists(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(UpdateProductDto updateProductDto)
        {
            throw new NotImplementedException();
        }
    }
}
