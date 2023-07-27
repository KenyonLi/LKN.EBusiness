using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Http.Client;
using Polly;
using System;

namespace LKN.EBusiness;

[DependsOn(
    typeof(EBusinessApplicationContractsModule),
    typeof(AbpAccountHttpApiClientModule),
    typeof(AbpIdentityHttpApiClientModule),
    typeof(AbpPermissionManagementHttpApiClientModule),
    typeof(AbpTenantManagementHttpApiClientModule),
    typeof(AbpFeatureManagementHttpApiClientModule),
    typeof(AbpSettingManagementHttpApiClientModule)
)]
public class EBusinessHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "EBusiness";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        //重试
        PreConfigure<AbpHttpClientBuilderOptions>(options => {
            options.ProxyClientBuildActions.Add((remoteServiceName,clientBuilder) => {
                clientBuilder.AddTransientHttpErrorPolicy(policyBuilder => {
                   return policyBuilder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, 1)));
                });
            });
        });   
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(EBusinessApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<EBusinessHttpApiClientModule>();
        });
    }
}
