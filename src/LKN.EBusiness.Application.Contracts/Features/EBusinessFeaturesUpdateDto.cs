using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.Features
{
    /// <summary>
    /// 电商项目特征更新Dto
    /// </summary>
    public class EBusinessFeaturesUpdateDto
    {
        public string IsEmail { set; get; }// 邮件特征(用户发送邮件)

        public string IsSms { set; get; } // 短信特征
    }
}
