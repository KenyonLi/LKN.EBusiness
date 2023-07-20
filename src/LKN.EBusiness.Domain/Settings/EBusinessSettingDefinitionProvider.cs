using Volo.Abp.Settings;

namespace LKN.EBusiness.Settings;

public class EBusinessSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EBusinessSettings.MySetting1));
    }
}
