using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
namespace LKN.EBusiness.Pays
{
    /// <summary>
    /// 微信支付核心类
    /// </summary>
    [Dependency(ServiceLifetime.Transient)]
    public class WxPayHttpClient 
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public WxPayHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        #region //微信支付请求
        /// <summary>
        /// 微信Post请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="requestString">参数</param>
        /// <param name="privateKey">商户证书 p12文件</param>
        /// <param name="merchantId">商户号</param>
        /// <param name="serialNo">商户证书号</param>
        /// <returns></returns>
        public async Task<string> WeChatPostAsync(string url, string requestString, string certPath, string merchantId, string serialNo)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");

                // 1、请求体构造
                var requestContent = new StringContent(requestString, mediaTypeHeaderValue);
               // requestContent.Headers.ContentType.MediaType = "application/json";

                // 2、请求头构造
                var auth = BuildAuthAsync(url, requestString, certPath, merchantId, serialNo, "POST");
                string value = $"WECHATPAY2-SHA256-RSA2048 {auth}";
                client.DefaultRequestHeaders.Add("Authorization", value);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "WOW64");
                client.Timeout = TimeSpan.FromSeconds(60);

                // 3、请求
                var response = await client.PostAsync(url, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    throw new Exception($"微信支付调用遭遇异常，原因：错误代码{response.StatusCode}，错误原因{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"微信支付调用遭遇异常，原因：" + ex.Message);
            }
        }
        /// <summary>
        /// 微信Get请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="requestString">参数</param>
        /// <param name="privateKey">私有证书 p12文件</param>
        /// <param name="merchantId">商户号</param>
        /// <param name="serialNo">商户证书号</param>
        /// <param name="method">Get或者Post</param>
        /// <returns></returns>
        public async Task<string> WeChatGetAsync(string url, string certPath, string merchantId, string serialNo)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var auth = BuildAuthAsync(url, "", certPath, merchantId, serialNo, "GET");
                string value = $"WECHATPAY2-SHA256-RSA2048 {auth}";
                client.DefaultRequestHeaders.Add("Authorization", value);
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("User-Agent", "WOW64");
                client.Timeout = TimeSpan.FromSeconds(60);
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    throw new Exception($"微信支付调用遭遇异常，原因：错误代码{response.StatusCode}，错误原因{response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"微信支付调用遭遇异常，原因：" + ex.Message);
            }
        }
        /// <summary>
        /// 1、构造Authorization
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonParame"></param>
        /// <param name="certPath"></param>
        /// <param name="merchantId"></param>
        /// <param name="serialNo"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        protected string BuildAuthAsync(string url, string jsonParame, string certPath, string merchantId, string serialNo, string method = "")
        {
            string body = jsonParame;
            var uri = new Uri(url);
            var urlPath = uri.PathAndQuery;
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            string nonce = Path.GetRandomFileName();

            string message = $"{method}\n{urlPath}\n{timestamp}\n{nonce}\n{body}\n";
            //string signature = Sign(message, privateKey);
           // var path = "apiclient_cert.p12";
            string signature = Sign(message, certPath, merchantId);
            return $"mchid=\"{merchantId}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{serialNo}\",signature=\"{signature}\"";
        }
        
        /// <summary>
        //// <summary>
        /// 2、签名
        /// </summary>
        /// <param name="data">要签名的数据</param>
        /// <param name="certPah">证书路径</param>
        /// <param name="certPwd">密码</param>
        /// <returns></returns>
        public string Sign(string data, string certPah, string certPwd)
        {
            var rsa = GetPrivateKey(certPah, certPwd);

            var rsaClear = new RSACryptoServiceProvider();

            var paras = rsa.ExportParameters(true);
            rsaClear.ImportParameters(paras);

            var signData = rsa.SignData(Encoding.UTF8.GetBytes(data), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signData);
        }

        
        /// <summary>
        /// 3、获取私钥
        /// </summary>
        /// <param name="priKeyFile">证书文件路径</param>
        /// <param name="keyPwd">密码</param>
        /// <returns></returns>
        private static RSA GetPrivateKey(string priKeyFile, string keyPwd)
        {
            var pc = new X509Certificate2(priKeyFile, keyPwd, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
            return (RSA)pc.PrivateKey;
        }
        #endregion
    }
}
