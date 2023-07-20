using LKN.EBusiness.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LKN.EBusiness.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EBusinessController : AbpControllerBase
{
    protected EBusinessController()
    {
        LocalizationResource = typeof(EBusinessResource);
    }
}
