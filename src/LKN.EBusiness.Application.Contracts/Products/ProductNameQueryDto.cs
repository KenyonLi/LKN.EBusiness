using Volo.Abp.Application.Dtos;

namespace LKN.EBusiness.Products
{
    public class ProductNameQueryDto : PagedAndSortedResultRequestDto
    {
        public string ProductName { set; get; }
    }
}
