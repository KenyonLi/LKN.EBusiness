using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKN.EBusiness.Pays
{
    /// <summary>
    /// 基本参数
    /// </summary>
    public class NativePay
    {
        public string mchid = "1613333188";
        public string appid = "wx31eff1a74e251e9e";
        public string out_trade_no = "native12177525012014070332333";
        public string description = "Image形象店-深圳腾大-QQ公仔";
        public string notify_url = "http://406286l2k9.wicp.vip/WxCallback";

        public NativePayPrice amount { get; } = new NativePayPrice();
    }
}
