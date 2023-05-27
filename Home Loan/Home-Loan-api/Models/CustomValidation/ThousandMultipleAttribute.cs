using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Home_Loan_api.Models.CustomValidation
{
    public class ThousandMultipleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int num;
                bool isInt = int.TryParse(value.ToString(), out num);
                if ( !isInt || num % 1000 != 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
