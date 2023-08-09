using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LKN.EBusiness.Languages
{
    /// <summary>
    /// 租户切换服务
    /// </summary>
    public interface ILanguageAppService
    {
        /// <summary>
        /// 获取语言
        /// </summary>
        public Task<IReadOnlyList<LanguageInfo>> GetAsync();

    }
}
