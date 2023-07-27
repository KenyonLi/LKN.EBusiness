using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LKN.OA;

[Dependency(ReplaceServices = true)]
public class OABrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OA";
}
