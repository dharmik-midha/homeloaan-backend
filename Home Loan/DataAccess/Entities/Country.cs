using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Entities
{
    public class Country
    {
       
            [Key]
            [Required]
            public string CountryId { get; set; }
            [Required]
            [MinLength(3)]
            [MaxLength(30)]
            public string CountryName { get; set; }

            [Required]
            [StringLength(3)]
            public string CountryCode { get; set; }

          
    }
}
