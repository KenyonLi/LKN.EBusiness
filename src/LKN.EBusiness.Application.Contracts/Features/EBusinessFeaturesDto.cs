using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.Features
{
    /// <summary>
    /// 电商项目特征
    /// </summary>
    public class EBusinessFeaturesDto
    {
        public string IsEmail { set; get; } // 邮件特征
        public string IsSms { set; get; } // 短信特征
    }
}
