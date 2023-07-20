using LKN.EBusiness.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace LKN.EBusiness.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class EBusinessPageModel : AbpPageModel
{
    protected EBusinessPageModel()
    {
        LocalizationResourceType = typeof(EBusinessResource);
    }
}
