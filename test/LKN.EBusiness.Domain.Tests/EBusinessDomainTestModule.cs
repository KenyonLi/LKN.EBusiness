using LKN.EBusiness.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LKN.EBusiness;

[DependsOn(
    typeof(EBusinessEntityFrameworkCoreTestModule)
    )]
public class EBusinessDomainTestModule : AbpModule
{

}
