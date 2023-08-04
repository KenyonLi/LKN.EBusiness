using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LKN.EBusiness.Features
{
    /// <summary>
    /// 定义特征
    /// </summary>
    public class EBusinessFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            // 1、特征组
            FeatureGroupDefinition featureGroupDefinition = context.AddGroup(EBusinessFeatures.GroupName);

            // 2、定义邮件特征
            featureGroupDefinition.AddFeature(EBusinessFeatures.Orders.IsEmail, "false");

            // 3、定义短信特征
            featureGroupDefinition.AddFeature(EBusinessFeatures.Orders.IsSms, "false");
        }
    }
}
