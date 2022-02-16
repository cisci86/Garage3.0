using Garage_2._0.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Validation
{
    public class NameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const string errorMassage = "First and Last name can not be the same!";
            
            if(value is string input)
            {
                var model = validationContext.ObjectInstance as Member;
                
                if(model != null)
                {
                    if (model.Name.FirstName != input)
                        return ValidationResult.Success;
                    else
                        return new ValidationResult(errorMassage);
                }
            }
            return new ValidationResult(errorMassage);
        }
    }
}
