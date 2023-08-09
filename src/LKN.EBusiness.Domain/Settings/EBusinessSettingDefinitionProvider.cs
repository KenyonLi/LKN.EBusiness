using Volo.Abp.Settings;

namespace LKN.EBusiness.Settings
{
    public class EBusinessSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(EBusinessSettings.MySetting1));
            // 1、定义微信支付设置
            context.Add(
                new SettingDefinition(EBusinessSettings.WxPay.NativeUrl),
                new SettingDefinition(EBusinessSettings.WxPay.Mchid),
                new SettingDefinition(EBusinessSettings.WxPay.Certpath),
                new SettingDefinition(EBusinessSettings.WxPay.CertSerialNo));
        }
    }
}
