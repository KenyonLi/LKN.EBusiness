using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LKN.EBusiness.Permissions
{
    /// <summary>
    /// 授权接口
    /// </summary>
    public interface IEBusinessPermissionsAppService
    {
        public Task AddRolePermissionAsync(string roleName, string permission);

        public Task AddUserPermissionAsync(Guid userId, string permission);
    }
}
