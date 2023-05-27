using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Entities
{
    public class City
    {
        [Key]
        [Required]
        public string CityId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string CityName { get; set; }

        [Required]
        [StringLength(3)]
        public string CityCode { get; set; }

        [Required]
        public string StateId { get; set; }
    }
}
