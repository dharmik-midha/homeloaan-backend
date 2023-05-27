using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

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
