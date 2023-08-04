using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKN.EBusiness.Features
{
    public static class EBusinessFeatures
    {
        public const string GroupName = "EBusiness";
        /// <summary>
        /// 邮件特征
        /// </summary>
        public static class Orders
        {
            public const string Default = GroupName + ".Orders";
            public const string IsEmail = Default + ".IsEmail";
            public const string IsSms = Default + ".IsSms";//发送短信特征
        }
    }
}
