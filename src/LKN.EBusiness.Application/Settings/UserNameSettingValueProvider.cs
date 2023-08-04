using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace LKN.EBusiness.Settings
{
    /// <summary>
    /// 用户名称设置自定义
    /// </summary>
    public class UserNameSettingValueProvider : SettingValueProvider
    {
        public const string ProviderName = "UR";

        public override string Name => ProviderName;

        protected ICurrentUser CurrentUser { get; }

        public UserNameSettingValueProvider(ISettingStore settingStore, ICurrentUser currentUser)
            : base(settingStore)
        {
            CurrentUser = currentUser;
        }

        public override async Task<string> GetOrNullAsync(SettingDefinition setting)
        {
            if (CurrentUser.UserName == null)
            {
                return null;
            }

            return await SettingStore.GetOrNullAsync(setting.Name, Name, CurrentUser.UserName.ToString());
        }

        public override async Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
        {
            if (CurrentUser.Id == null)
            {
                return settings.Select(x => new SettingValue(x.Name, null)).ToList();
            }

            return await SettingStore.GetAllAsync(settings.Select(x => x.Name).ToArray(), Name, CurrentUser.UserName.ToString());
        }
    }
}
