using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.Interceptors
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class LogAttribute:Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public LogAttribute() { }
    }
}
