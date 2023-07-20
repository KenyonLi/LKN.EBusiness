using System;
using System.Collections.Generic;
using System.Text;
using LKN.EBusiness.Localization;
using Volo.Abp.Application.Services;

namespace LKN.EBusiness;

/* Inherit your application services from this class.
 */
public abstract class EBusinessAppService : ApplicationService
{
    protected EBusinessAppService()
    {
        LocalizationResource = typeof(EBusinessResource);
    }
}
