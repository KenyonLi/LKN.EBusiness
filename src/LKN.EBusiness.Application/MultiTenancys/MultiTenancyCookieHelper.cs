using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LKN.EBusiness.MultiTenancys
{
    /// <summary>
    /// 
    /// </summary>
    public static class MultiTenancyCookieHelper
    {
        public static void SetTenantCookie(
            HttpContext context,
            Guid? tenantId,
            string tenantKey)
        {
            if (tenantId != null)
            {
                context.Response.Cookies.Append(
                    tenantKey,
                    tenantId.ToString(),
                    new CookieOptions
                    {
                        Path = "/",
                        HttpOnly = false,
                        Expires = DateTimeOffset.Now.AddYears(10)
                    }
                );
            }
            else
            {
                context.Response.Cookies.Delete(tenantKey);
            }
        }
    }
}
