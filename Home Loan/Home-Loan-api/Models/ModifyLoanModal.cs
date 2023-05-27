using Home_Loan_api.Models.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Home_Loan_api.Models
{
    public class ModifyLoanModal
    {

        [Required]
        [MaxLength(100, ErrorMessage = "Address can only be 100 characters long.")]
        public string PropertyAddress { get; set; }


        [Required]
        [Range(0, 100000000, ErrorMessage = "Property size must be Greater than 0.")]
        [WholeNumber(ErrorMessage = "Property size must be whole number.")]
        public int PropertySize { get; set; }


        [Required]
        [ThousandMultiple(ErrorMessage = "Cost must be multiple of thousand.")]
        [Range(1000, 100000000, ErrorMessage = "Loan amount should be between 1k to 10Cr")]
        public int PropertyCost { get; set; }


        [Required]
        [Range(1000, 100000000, ErrorMessage = "Loan amount should be between 1K to 10Cr")]
        public float PropertyRegistrationCost { get; set; }


        [Required]
        [Range(1000, 100000000, ErrorMessage = "Loan amount shoud be between 1K to 10Cr")]
        public float MonthlyFamilyIncome { get; set; }

        [Range(0, 100000000, ErrorMessage = "Please enter a valid value.")]
        public float OtherIncome { get; set; }


        [Required]
        [Range(100000, 100000000, ErrorMessage = "Loan amount shoud be between 1Lac to 10Cr")]
        public float LoanAmount { get; set; }


        [Required]
        [Range(12, 240, ErrorMessage = "Loan Duration should be between 12 months to 240 months")]
        public int LoanDuration { get; set; }
    }
}
