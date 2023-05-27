using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Home_Loan_api.Models
{
    public class AddAddressModal
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string CityName { get; set; }

        [Required]
        [StringLength(3)]
        public string CityCode { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string CountryName { get; set; }

        [Required]
        [StringLength(3)]
        public string CountryCode { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string StateName { get; set; }

        [Required]
        [StringLength(3)]
        public string StateCode { get; set; }
    }
}
