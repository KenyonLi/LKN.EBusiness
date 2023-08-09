using LKN.EBusiness.Pays;
using LKN.EBusiness.Permissions;
using LKN.EBusiness.Settings;
using LKN.EBusiness.Validations;
using Volo.Abp.Account;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Http.Client;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace LKN.EBusiness;

[DependsOn(
    typeof(EBusinessDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(EBusinessApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAutofacModule),
    typeof(AbpHttpClientModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class EBusinessApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<EBusinessApplicationModule>();
        });



        // 自定义权限校验处理
        Configure<AbpPermissionOptions>(options =>
        {
            options.ValueProviders.Add<UserEmailPermissionValueProvider>();
        });

        // 自定义权限管理
        Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<UserEmailPermissionManagementProvider>();
        });

        // 自定义多租户解析器
        /*Configure<AbpTenantResolveOptions>(options =>
        {
            options.TenantResolvers.Add(new RedisTenantResolveContributor());
        });*/

        //微信支付配置
        // 使用configurtion 从配置文件中获取配置值
        // appsettings.json配置文件缺点
        // 1、无法复用
        // 2、维护难度大

        Configure<WxPayOptions>(options => {
            options.nativeUrl = "https://api.mch.weixin.qq.com/v3/pay/transactions/native";
            options.mchid = "1613333188";
            options.certpath = @"D:\work\net-project\ABP专题\4、核心项目-电商项目模块原理分析\YDT.EBusiness\src\YDT.EBusiness.Application\Pays\certs\apiclient_cert.p12";
            options.certSerialNo = "6FC4BB506EC38075C5F4F160885ED655A0604DC6";
        });

        Configure<AbpValidationOptions>(options => {
            options.ObjectValidationContributors.Add<ProductObjectValidationContributor>();
        });

        // 虚拟文件系统
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<EBusinessApplicationModule>(
                baseNamespace: "LKN.EBusiness.Application",
                baseFolder: "Pays/certs"
            );
        });

        // 配置设置自定义管理提供者
        Configure<SettingManagementOptions>(options =>
        {
            options.Providers.Add<UserNameSettingManagementProvider>();
        });

        Configure<AbpSettingOptions>(options =>
        {
            options.ValueProviders.Add<UserNameSettingValueProvider>();
        });
    }
}
