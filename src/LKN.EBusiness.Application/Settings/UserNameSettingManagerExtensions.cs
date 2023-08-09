using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace LKN.EBusiness.Settings
{
    /// <summary>
    ///  用户名设置自定义扩展
    /// </summary>

    public static class UserNameSettingManagerExtensions
    {
        public static Task<string> GetOrNullForUserNameAsync(this ISettingManager settingManager, [NotNull] string name, string userName, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, UserNameSettingValueProvider.ProviderName, userName.ToString(), fallback);
        }

        public static Task<List<SettingValue>> GetAllForUserNameAsync(this ISettingManager settingManager, string userName, bool fallback = true)
        {
            return settingManager.GetAllAsync(UserNameSettingValueProvider.ProviderName, userName.ToString(), fallback);
        }

        public static Task SetForUserNameAsync(this ISettingManager settingManager, string userName, [NotNull] string name, [CanBeNull] string value, bool forceToSet = false)
        {
            return settingManager.SetAsync(name, value, UserNameSettingValueProvider.ProviderName, userName, forceToSet);
        }

    }
}