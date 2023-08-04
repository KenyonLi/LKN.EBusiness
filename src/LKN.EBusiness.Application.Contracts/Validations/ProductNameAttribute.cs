using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LKN.EBusiness.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ProductNameAttribute : ValidationAttribute
    {
        public ProductNameAttribute()
        {

        }
    }
}
