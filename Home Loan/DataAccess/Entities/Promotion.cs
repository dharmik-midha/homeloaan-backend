using Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Entities
{
    public class Promotion
    {
        [Key]
        [Required]
        public  string PromotionId { get; set; }

        [Required]
        public bool Active { get; set; }


        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Start_date { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime End_date { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(200)]
        public string Message { get; set; }

        [Required]
        public PromotionTypes Type { get; set; }
    }
}
