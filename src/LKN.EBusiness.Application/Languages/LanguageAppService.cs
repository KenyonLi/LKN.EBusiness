using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LKN.EBusiness.Languages
{
    public class LanguageAppService : EBusinessAppService, ILanguageAppService
    {

        public ILanguageProvider languageProvider { set; get; }

        public Task<IReadOnlyList<LanguageInfo>> GetAsync()
        {
            return languageProvider.GetLanguagesAsync();
        }
    }
}
