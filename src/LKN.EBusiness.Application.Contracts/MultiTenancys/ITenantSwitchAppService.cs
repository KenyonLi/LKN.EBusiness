using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.MultiTenancys
{
    public interface ITenantSwitchAppService
    {
        /// <summary>
        /// 切换租户
        /// </summary>
        public void SwitchTenant(TenantSwitchInputDto tenantSwitchInputDto);
    }
}
