using Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.DTO
{
    public class LoanDTO
    {
        public string LoanId { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }


        [Required]
        public string PropertyAddress { get; set; }


        [Required]
        public int PropertySize { get; set; }


        [Required]
        [Range(100000, 100000000, ErrorMessage = "Loan amount should be between 1Lac to 10Cr")]
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
        [Range(1, 30, ErrorMessage = "Loan Duration should be between 1yr to 30yrs")]
        public int LoanDuration { get; set; }

        public LoanStates state { get; set; }
    }
}
