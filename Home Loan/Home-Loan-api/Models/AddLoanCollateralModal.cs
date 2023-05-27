using System;
using Constants.Enums;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace Home_Loan_api.Models
{
    public class AddLoanCollateralModal
    {
        [Required]
        public string CollateralId { get; set; }
    }
}
