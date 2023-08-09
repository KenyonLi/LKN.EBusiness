using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LKN.EBusiness.MultiTenancys
{
    /// <summary>
    /// 多租户 redis自定义解析
    /// </summary>
    public class RedisTenantResolveContributor : TenantResolveContributorBase
    {
        public override string Name => "redis";

        public override Task ResolveAsync(ITenantResolveContext context)
        {
            // 1、获取tanentKey
            string tanentKey = TenantResolverConsts.DefaultTenantKey;

            // 2、从redis取值

            // 3、把值设置到上下文
            context.TenantIdOrName = "";

            return Task.CompletedTask;
        }
    }
}
