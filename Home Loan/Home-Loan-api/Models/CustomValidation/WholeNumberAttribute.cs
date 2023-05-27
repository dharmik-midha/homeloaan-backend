using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Home_Loan_api.Models.CustomValidation
{
    public class WholeNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                bool isWhole = Regex.IsMatch(value.ToString(), "([0-9]+)");
                if (!isWhole)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
