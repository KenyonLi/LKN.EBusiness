using LKN.EBusiness.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LKN.EBusiness.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EBusinessEntityFrameworkCoreModule),
    typeof(EBusinessApplicationContractsModule)
    )]
public class EBusinessDbMigratorModule : AbpModule
{
}
