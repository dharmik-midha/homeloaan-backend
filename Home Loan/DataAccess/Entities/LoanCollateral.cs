using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class LoanCollateral
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string LoanId { get; set; }

        [Required]
        public string CollateralId { get; set; }
    }
}
