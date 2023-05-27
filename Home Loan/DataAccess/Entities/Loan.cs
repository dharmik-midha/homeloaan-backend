using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Entities
{
    public class Loan
    {
        [Key]
        public string LoanId { get; set; }


        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        [DataType(DataType.EmailAddress)]
        public string userEmail { get; set; }


        [Required]
        public string PropertyAddress { get; set; }


        [Required]
        [Range(0, 100000000, ErrorMessage = "Property size must be Greater than 0.")]
        public int PropertySize { get; set; }


        [Required]
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
