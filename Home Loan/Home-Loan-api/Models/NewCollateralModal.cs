using System;
using Constants.Enums;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace Home_Loan_api.Models
{
    public class NewCollateralModal
    {
        [Required]
        [Range(100000, 100000000, ErrorMessage = "Collateral value should be in range 1Lac to 10Cr")]
        public float CollateralValue { get; set; }

        [Required]
        public CollateralTypes CollateralType { get; set; }

        [Required]
        [Range(0, 100)]
        public int OwnShare { get; set; }
    }
}
