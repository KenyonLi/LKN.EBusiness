using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;
using Volo.Abp.Users;

namespace LKN.EBusiness.Settings
{
    /// <summary>
    /// 用户角色设置自定义管理
    /// </summary>
    public class UserNameSettingManagementProvider : SettingManagementProvider, ITransientDependency
    {
        public override string Name => UserNameSettingValueProvider.ProviderName;

        protected ICurrentUser CurrentUser { get; }

        public UserNameSettingManagementProvider(
            ISettingManagementStore settingManagementStore,
            ICurrentUser currentUser)
            : base(settingManagementStore)
        {
            CurrentUser = currentUser;
        }

        protected override string NormalizeProviderKey(string providerKey)
        {
            if (providerKey != null)
            {
                return providerKey;
            }

            return CurrentUser.UserName?.ToString();
        }
    }
}