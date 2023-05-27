using Home_Loan_api.Models.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Home_Loan_api.Models
{
    public class PasswordResetModal
    {
        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        [ValidPassword]
        public string CurrentPassword { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        [ValidPassword]
        public string NewPassword { get; set; }
    }
}
