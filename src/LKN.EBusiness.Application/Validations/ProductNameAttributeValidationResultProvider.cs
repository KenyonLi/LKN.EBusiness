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
    /// 自定义校验特性判断
    /// </summary>
   // [Dependency(ServiceLifetime.Transient)]
    public class ProductNameAttributeValidationResultProvider : IAttributeValidationResultProvider
    {
        public ValidationResult GetOrDefault(ValidationAttribute validationAttribute, object validatingObject, ValidationContext validationContext)
        {
            return validationAttribute.GetValidationResult(validatingObject, validationContext);
        }
    }
}
