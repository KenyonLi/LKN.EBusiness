using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace LKN.EBusiness.MultiTenancys
{
    /// <summary>
    /// 租户转换服务
    /// </summary>
    public class TenantSwitchAppService : EBusinessAppService, ITenantSwitchAppService
    {
        public ITenantStore TenantStore { set; get; }

        public IHttpContextAccessor httpContextAccessor { get; set; }
        public void SwitchTenant(TenantSwitchInputDto tenantSwitchInputDto)
        {
            var guid = CurrentTenant.Id;
            Guid? tenantId = null;
            if (!tenantSwitchInputDto.Name.IsNullOrEmpty())
            {
                var tenant = TenantStore.Find(tenantSwitchInputDto.Name);
                if (tenant == null)
                {
                    throw new BusinessException($"{tenantSwitchInputDto.Name}，租户不存在");
                }

                if (!tenant.IsActive)
                {
                    throw new BusinessException($"{tenantSwitchInputDto.Name}，租户未激活");
                }

                tenantId = tenant.Id;
            }

            MultiTenancyCookieHelper.SetTenantCookie(httpContextAccessor.HttpContext, tenantId, TenantResolverConsts.DefaultTenantKey);
        }
    }
}
