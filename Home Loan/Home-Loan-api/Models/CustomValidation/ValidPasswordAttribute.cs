using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Home_Loan_api.Models.CustomValidation
{
    public class ValidPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                bool hasUpper = Regex.IsMatch(value.ToString(), "[A-Z]");
                bool hasLower = Regex.IsMatch(value.ToString(), "[a-z]");
                bool hasNumber = Regex.IsMatch(value.ToString(), @"\d");
                bool hasSpecial = Regex.IsMatch(value.ToString(), @"[!@#$%&:?]");
                if (!hasUpper || !hasLower || !hasNumber || !hasSpecial)
                {
                    string errmsg = "The Password must contain ";
                    if (!hasLower) errmsg += "One Lower Case ";
                    if (!hasUpper) errmsg += "One Upper Case ";
                    if (!hasNumber) errmsg += "One Digit ";
                    if (!hasSpecial) errmsg += "One Special Character( !@#$%&:? ).";
                    return new ValidationResult(errmsg);
                }
            }
            return ValidationResult.Success;
        }
    }
}
