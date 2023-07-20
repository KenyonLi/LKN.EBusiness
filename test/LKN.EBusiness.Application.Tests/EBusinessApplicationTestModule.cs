using Volo.Abp.Modularity;

namespace LKN.EBusiness;

[DependsOn(
    typeof(EBusinessApplicationModule),
    typeof(EBusinessDomainTestModule)
    )]
public class EBusinessApplicationTestModule : AbpModule
{

}
