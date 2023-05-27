using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using Constants.Enums;

namespace DataAccess.Entities
{
    public class LoanState
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string LoanID { get; set; }

        [Required]
        public LoanStates State { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserID { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }
    }
}
