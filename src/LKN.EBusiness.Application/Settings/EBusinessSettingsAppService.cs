using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.SettingManagement;

namespace LKN.EBusiness.Settings
{
    /// <summary>
    /// 电商项目设置实现
    /// </summary>
    public class EBusinessSettingsAppService : EBusinessAppService, IEBusinessSettingsAppService
    {
        public ISettingManager _settingManager { set; get; }

        public async Task<EBusinessSettingsDto> GetAsync()
        {
            #region 1、获取设置
            {
                EBusinessSettingsDto eBusinessSettingsDto = new EBusinessSettingsDto();
                eBusinessSettingsDto.nativeUrl = await _settingManager.GetOrNullGlobalAsync(EBusinessSettings.WxPay.NativeUrl);
                eBusinessSettingsDto.mchid = await _settingManager.GetOrNullGlobalAsync(EBusinessSettings.WxPay.Mchid);
                eBusinessSettingsDto.certpath = await _settingManager.GetOrNullGlobalAsync(EBusinessSettings.WxPay.Certpath);
                eBusinessSettingsDto.certSerialNo = await _settingManager.GetOrNullGlobalAsync(EBusinessSettings.WxPay.CertSerialNo);

                return eBusinessSettingsDto;
            }
            #endregion

        }


        public async Task UpdateAsync(EBusinessSettingsUpdateDto input)
        {
            #region 1、全局设置
            {
                await _settingManager.SetGlobalAsync(EBusinessSettings.WxPay.NativeUrl, input.nativeUrl);
                await _settingManager.SetGlobalAsync(EBusinessSettings.WxPay.Mchid, input.mchid);
                await _settingManager.SetGlobalAsync(EBusinessSettings.WxPay.Certpath, input.certpath);
                await _settingManager.SetGlobalAsync(EBusinessSettings.WxPay.CertSerialNo, input.certSerialNo);
            }
            #endregion


        }

        /// <summary>
        /// 多租户获取设置
        /// </summary>
        /// <returns></returns>
        public async Task<EBusinessSettingsDto> GetTenantsettingAsync()
        {

            #region 2、多租户获取设置
            {
                EBusinessSettingsDto eBusinessSettingsDto = new EBusinessSettingsDto();
                eBusinessSettingsDto.nativeUrl = await _settingManager.GetOrNullForTenantAsync(EBusinessSettings.WxPay.NativeUrl, (Guid)CurrentTenant.Id);
                eBusinessSettingsDto.mchid = await _settingManager.GetOrNullForTenantAsync(EBusinessSettings.WxPay.Mchid, (Guid)CurrentTenant.Id);
                eBusinessSettingsDto.certpath = await _settingManager.GetOrNullForTenantAsync(EBusinessSettings.WxPay.Certpath, (Guid)CurrentTenant.Id);
                eBusinessSettingsDto.certSerialNo = await _settingManager.GetOrNullForTenantAsync(EBusinessSettings.WxPay.CertSerialNo, (Guid)CurrentTenant.Id);

                return eBusinessSettingsDto;
            }
            #endregion
        }

        /// <summary>
        /// 多租设置设置
        /// </summary>
        public async Task UpdateTenantsettingAsync(EBusinessSettingsUpdateDto input)
        {
            #region 2、多租户设置
            {
                await _settingManager.SetForTenantAsync((Guid)CurrentTenant.Id, EBusinessSettings.WxPay.NativeUrl, input.nativeUrl);
                await _settingManager.SetForTenantAsync((Guid)CurrentTenant.Id, EBusinessSettings.WxPay.Mchid, input.mchid);
                await _settingManager.SetForTenantAsync((Guid)CurrentTenant.Id, EBusinessSettings.WxPay.Certpath, input.certpath);
                await _settingManager.SetForTenantAsync((Guid)CurrentTenant.Id, EBusinessSettings.WxPay.CertSerialNo, input.certSerialNo);
            }
            #endregion

        }

        /// <summary>
        /// 用户名设置获取
        /// </summary>
        /// <returns></returns>
        public async Task<EBusinessSettingsDto> GetUsernamesettingAsync()
        {

            #region 3、用户名获取设置
            {
                EBusinessSettingsDto eBusinessSettingsDto = new EBusinessSettingsDto();
                eBusinessSettingsDto.nativeUrl = await _settingManager.GetOrNullForUserNameAsync(EBusinessSettings.WxPay.NativeUrl, CurrentUser.UserName);
                eBusinessSettingsDto.mchid = await _settingManager.GetOrNullForUserNameAsync(EBusinessSettings.WxPay.Mchid, CurrentUser.UserName);
                eBusinessSettingsDto.certpath = await _settingManager.GetOrNullForUserNameAsync(EBusinessSettings.WxPay.Certpath, CurrentUser.UserName);
                eBusinessSettingsDto.certSerialNo = await _settingManager.GetOrNullForUserNameAsync(EBusinessSettings.WxPay.CertSerialNo, CurrentUser.UserName);

                return eBusinessSettingsDto;
            }
            #endregion
        }

        /// <summary>
        /// 用户名设置添加
        /// </summary>
        public async Task UpdateUsernamesettingAsync(EBusinessSettingsUpdateDto input)
        {
            #region 3、用户名设置
            {
                await _settingManager.SetForUserNameAsync(CurrentUser.UserName, EBusinessSettings.WxPay.NativeUrl, input.nativeUrl);
                await _settingManager.SetForUserNameAsync(CurrentUser.UserName, EBusinessSettings.WxPay.Mchid, input.mchid);
                await _settingManager.SetForUserNameAsync(CurrentUser.UserName, EBusinessSettings.WxPay.Certpath, input.certpath);
                await _settingManager.SetForUserNameAsync(CurrentUser.UserName, EBusinessSettings.WxPay.CertSerialNo, input.certSerialNo);
            }
            #endregion

        }

    }
}
