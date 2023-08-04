using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LKN.EBusiness.Settings
{
    /// <summary>
    /// 电商项目设置
    /// </summary>
    public interface IEBusinessSettingsAppService : IApplicationService
    {
        Task<EBusinessSettingsDto> GetAsync();

        Task UpdateAsync(EBusinessSettingsUpdateDto input);
    }
}
