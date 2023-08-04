namespace LKN.EBusiness.Settings
{
    /// <summary>
    /// 电商项目更新Dto
    /// </summary>
    public class EBusinessSettingsUpdateDto
    {
        public string nativeUrl { set; get; }// 支付接口
        public string mchid { set; get; }// 商户Id
        public string certpath { set; get; } // 商户证书路径
        public string certSerialNo { set; get; }// 证书序列号
    }
}
