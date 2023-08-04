using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;

namespace LKN.EBusiness.Pays
{
    public interface IPayAppService: IApplicationService
    {
        /// <summary>
        /// 支付创建
        /// </summary>
        /// <param name="OrderSn"></param>
        /// <param name="OrderPrice"></param>
        /// <returns></returns>
        public string CreatePay(string productName, string orderSn, string totalPrice);
    }
}
