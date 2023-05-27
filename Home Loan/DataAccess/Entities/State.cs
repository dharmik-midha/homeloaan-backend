using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Entities
{
    public class State
    {
       
            [Key]
            [Required]
            public string StateId { get; set; }
            [Required]
            [MinLength(3)]
            [MaxLength(30)]
            public string StateName { get; set; }

            [Required]
            [StringLength(3)]
            public string StateCode { get; set; }

            [Required]
            public string CountryId { get; set; }

    }
    
}
