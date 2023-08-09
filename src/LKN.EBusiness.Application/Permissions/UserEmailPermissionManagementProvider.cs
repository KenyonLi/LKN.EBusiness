using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using LKN.EBusiness.Permissions;

namespace LKN.EBusiness
{
    /// <summary>
    /// UserEmail 基于用户邮件权限
    /// </summary>
    public class UserEmailPermissionManagementProvider : PermissionManagementProvider
    {
        public UserEmailPermissionManagementProvider(IPermissionGrantRepository permissionGrantRepository, IGuidGenerator guidGenerator, ICurrentTenant currentTenant) : base(permissionGrantRepository, guidGenerator, currentTenant)
        {
        }

        public override string Name => "UE"; // 代表用户邮件
    }
}
