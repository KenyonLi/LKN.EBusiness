using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace LKN.EBusiness.Validations
{
    /// <summary>
    /// 商品名称验证
    /// </summary>
   // [Dependency(ServiceLifetime.Transient)]
    public class ProductObjectValidationContributor : IObjectValidationContributor,ITransientDependency
    {
        //public void AddErrors(ObjectValidationContext context)
        //{
        //    object _object = context.ValidatingObject;

        //    //context.Errors.Add(new ValidationResult("信息错误"));
        //}
        public Task AddErrorsAsync(ObjectValidationContext context)
        {
            object _object = context.ValidatingObject;
            return Task.CompletedTask;
            // throw new NotImplementedException();
        }
    }
}
