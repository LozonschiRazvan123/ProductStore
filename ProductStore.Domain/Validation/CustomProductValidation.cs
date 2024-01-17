using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Validation
{
    public class CustomProductValidation: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int price && price>0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Price must be greater than zero.");
        }
    }
}
