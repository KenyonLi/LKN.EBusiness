using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;

namespace LKN.EBusiness.Permissions
{
    /// <summary>
    /// 1、自定义权限校验
    /// </summary>
    class UserEmailPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "UE";

        public UserEmailPermissionValueProvider(IPermissionStore permissionStore) : base(permissionStore)
        {
        }

        public override string Name => ProviderName;

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            // 1、从登录里面获取邮箱
            var Email = context.Principal?.FindFirst(AbpClaimTypes.Email)?.Value;

            if (Email == null)
            {
                return PermissionGrantResult.Undefined;
            }

            // 2、从权限表查询是否有权限
            return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, Email)
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }

        public override async Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
        {
            var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToArray();
            Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

            var Email = context.Principal?.FindFirst(AbpClaimTypes.Email)?.Value;
            if (Email == null)
            {
                return new MultiplePermissionGrantResult(permissionNames);
            }

            return await PermissionStore.IsGrantedAsync(permissionNames, Name, Email);
        }
    }
}
