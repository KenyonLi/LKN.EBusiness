using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LKN.EBusiness.Interceptors
{
    /// <summary>
    /// 邮件拦截器
    /// </summary>
    [Dependency(ServiceLifetime.Transient)]
    public class LogInterceptor : AbpInterceptor
    {
        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            Console.WriteLine("方法操作前日志");
            await  invocation.ProceedAsync();
            Console.WriteLine("方法操作后日志");
        }
    }
}
