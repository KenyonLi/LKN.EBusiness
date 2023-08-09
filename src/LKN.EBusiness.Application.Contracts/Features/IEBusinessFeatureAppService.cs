using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LKN.EBusiness.Features
{
    /// <summary>
    /// 电商项目特征
    /// </summary>
    public interface IEBusinessFeatureAppService : IApplicationService
    {
        Task<EBusinessFeaturesDto> GetAsync();
        Task UpdateAsync(EBusinessFeaturesUpdateDto input);
    }
}
