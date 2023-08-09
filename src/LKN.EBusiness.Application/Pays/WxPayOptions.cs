using System;
using System.Collections.Generic;

namespace LKN.EBusiness.Pays
{
    public class WxPayOptions
    {
        public string nativeUrl { set; get; }// 支付接口
        public  string mchid { set; get; }// 商户Id
        public  string certpath { set; get; } // 商户证书路径
        public  string certSerialNo { set; get; }// 证书序列号

        public WxPayOptions()
        {
            nativeUrl = "https://api.mch.weixin.qq.com/v3/pay/transactions/native";
            mchid = "1613333188";
            certpath = @"/apiclient_cert.p12";
            certSerialNo = "6FC4BB506EC38075C5F4F160885ED655A0604DC6";
        }
    }
}
