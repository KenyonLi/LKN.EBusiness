using AutoMapper;
using LKN.EBusiness.Products;
using Volo.Abp.Application.Dtos;

namespace LKN.EBusiness;

public class EBusinessApplicationAutoMapperProfile : Profile
{
    public EBusinessApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Product, ProductDto>();
        CreateMap<PagedAndSortedResultRequestDto, Product>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<ProductAttrQueryDto, Product>();
        CreateMap<ProductImageDto, ProductImage>();
        CreateMap<ProductImage, ProductImageDto>();
    }
}
