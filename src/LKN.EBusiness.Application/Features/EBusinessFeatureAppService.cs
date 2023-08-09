using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.FeatureManagement;

namespace LKN.EBusiness.Features
{
    /// <summary>
    /// 电商项目特征实现
    /// </summary>
    public class EBusinessFeatureAppService : EBusinessAppService, IEBusinessFeatureAppService
    {
        public IFeatureManager _featureManager { set; get; }

        public async Task<EBusinessFeaturesDto> GetAsync()
        {
            EBusinessFeaturesDto eBusinessFeaturesDto = new EBusinessFeaturesDto();
            eBusinessFeaturesDto.IsEmail = await _featureManager.GetOrNullForTenantAsync(EBusinessFeatures.Orders.IsEmail, (Guid)CurrentTenant.Id);
            eBusinessFeaturesDto.IsSms = await _featureManager.GetOrNullForTenantAsync(EBusinessFeatures.Orders.IsSms, (Guid)CurrentTenant.Id);
            return eBusinessFeaturesDto;
        }

        public async Task UpdateAsync(EBusinessFeaturesUpdateDto input)
        {
            await _featureManager.SetForTenantAsync((Guid)CurrentTenant.Id, EBusinessFeatures.Orders.IsEmail, input.IsEmail);
            await _featureManager.SetForTenantAsync((Guid)CurrentTenant.Id, EBusinessFeatures.Orders.IsSms, input.IsSms);
        }
    }
}