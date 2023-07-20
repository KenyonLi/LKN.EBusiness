using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace LKN.EBusiness.Web;

[Dependency(ReplaceServices = true)]
public class EBusinessBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "EBusiness";
}
