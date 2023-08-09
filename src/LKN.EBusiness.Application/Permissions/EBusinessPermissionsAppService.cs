using LKN.EBusiness.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;

namespace LKN.EBusiness.Users
{
    /// <summary>
    /// 权限服务
    /// </summary>
    [Authorize]
    public class EBusinessPermissionsAppService: EBusinessAppService,IEBusinessPermissionsAppService/*, IApplicationService, ITransientDependency*/
    {
        private readonly IPermissionManager _permissionManager;

        public EBusinessPermissionsAppService(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        public async Task AddRolePermissionAsync(string roleName, string permission)
        {
            await _permissionManager
            .SetForRoleAsync(roleName, permission, true);
        }

        public async Task AddUserPermissionAsync(Guid userId, string permission)
        {
            await _permissionManager
            .SetForUserAsync(userId, permission, true);
        }

        public async Task AddUserEamilPermissionAsync(string Eamil, string permission)
        {
            await _permissionManager.SetAsync(permission, UserEmailPermissionValueProvider.ProviderName, Eamil,true);
        }
    }
}
