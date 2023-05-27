using Home_Loan_api.Models.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Home_Loan_api.Models
{
    public class RegisterModal
    {
        [MinLength(5)]
        [MaxLength(30)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        public string Email{ get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        [ValidPassword]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        public string Phone { get; set; }

        [Required]
        [StringLength(3)]
        public string CityCode { get; set; }

        [Required]
        [StringLength(3)]
        public string StateCode { get; set; }

        [Required]
        [StringLength(3)]
        public string CountryCode { get; set; }
    }
}
