using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Validation
{
    public sealed class CheckPrice: IValidatableObject
    {
        public int Price { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Price <= 0)
            {
                yield return new ValidationResult("Price must be greater than zero.", new[] { nameof(Price) });
            }
        }
    }
}
