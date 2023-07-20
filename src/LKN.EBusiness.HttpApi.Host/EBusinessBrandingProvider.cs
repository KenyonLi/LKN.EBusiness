using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LKN.EBusiness;

[Dependency(ReplaceServices = true)]
public class EBusinessBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "EBusiness";
}
