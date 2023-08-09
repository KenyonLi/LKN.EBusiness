using LKN.EBusiness.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.VirtualFileSystem;

namespace LKN.EBusiness.Pays
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class WxPayAppService : EBusinessAppService, IPayAppService
    {
        public WxPayHttpClient _wxPayHttpClient { set; get; }

        private const string nativeUrl = "https://api.mch.weixin.qq.com/v3/pay/transactions/native";// 支付接口
        private const string mchid = "1613333188"; // 商户Id
        private const string certpath = @"D:\work\net-project\ABP专题\4、核心项目-电商项目模块原理分析\YDT.EBusiness\src\YDT.EBusiness.Application\Pays\certs\apiclient_cert.p12"; // 商户证书路径
        private const string certSerialNo = "6FC4BB506EC38075C5F4F160885ED655A0604DC6"; // 证书序列号

        protected WxPayOptions _wxPayOptions { get; }
        public IVirtualFileProvider _virtualFileProvider { set; get; }

        public WxPayAppService(IOptions<WxPayOptions> wxPayOptions)
        {
            _wxPayOptions = wxPayOptions.Value;
        }

        public string CreatePay(string productName, string orderSn, string totalPrice)
        {
            #region 1、默认支付
            {
                //var file = _virtualFileProvider.GetFileInfo("/Pays/certs/apiclient_cert.p12");
                var file = _virtualFileProvider.GetFileInfo("/apiclient_cert.p12");
                _virtualFileProvider.GetDirectoryContents("/");
                // 1、创建支付对象
                NativePay nativePay = new NativePay();
                nativePay.description = productName;
                nativePay.out_trade_no = orderSn;
                nativePay.amount.total = int.Parse(float.Parse(totalPrice) * 100 + "");

                // 2、支付对象转换成json
                string nativePayJson = JsonConvert.SerializeObject(nativePay);

                // 3、创建支付
                string result = _wxPayHttpClient.WeChatPostAsync(nativeUrl,
                    nativePayJson,
                    file.PhysicalPath,
                    mchid,
                   certSerialNo).Result;
                return result;
            }
            #endregion

            #region 2、选项支付
            {
                /*  // 1、创建支付对象
                  NativePay nativePay = new NativePay();
                  nativePay.description = productName;
                  nativePay.out_trade_no = orderSn;
                  nativePay.amount.total = int.Parse(float.Parse(totalPrice) * 100 + "");

                  // 2、支付对象转换成json
                  string nativePayJson = JsonConvert.SerializeObject(nativePay);

                  // 3、创建支付
                  string result = _wxPayHttpClient.
                      WeChatPostAsync(_wxPayOptions.nativeUrl,
                      nativePayJson,
                      _wxPayOptions.certpath,
                      _wxPayOptions.mchid,
                     _wxPayOptions.certSerialNo).Result;

                  return result;*/

            }
            #endregion

            #region 3、设置支付
            {
                // 1、创建支付对象
                //NativePay nativePay = new NativePay();
                //nativePay.description = productName;
                //nativePay.out_trade_no = orderSn;
                //nativePay.amount.total = int.Parse(float.Parse(totalPrice) * 100 + "");

                //// 2、支付对象转换成json
                //string nativePayJson = JsonConvert.SerializeObject(nativePay);

                //// 3、创建支付
                //string a1 = SettingProvider.GetOrNullAsync(EBusinessSettings.WxPay.NativeUrl).Result;
                //string a2 = SettingProvider.GetOrNullAsync(EBusinessSettings.WxPay.Certpath).Result;
                //string a3 = SettingProvider.GetOrNullAsync(EBusinessSettings.WxPay.Mchid).Result;
                //string a4 = SettingProvider.GetOrNullAsync(EBusinessSettings.WxPay.CertSerialNo).Result;
                ////string result = _wxPayHttpClient.
                ////    WeChatPostAsync(SettingProvider.GetOrNullAsync(EBusinessSettings.WxPay.NativeUrl).Result,
                ////    nativePayJson,
                ////    SettingProvider.GetOrNullAsync(EBusinessSettings.WxPay.Certpath).Result,
                ////    SettingProvider.GetOrNullAsync(EBusinessSettings.WxPay.Mchid).Result,
                ////   SettingProvider.GetOrNullAsync(EBusinessSettings.WxPay.CertSerialNo).Result).Result;

                ////return result;
                //return "";
            }
            #endregion

        }
    }
}
