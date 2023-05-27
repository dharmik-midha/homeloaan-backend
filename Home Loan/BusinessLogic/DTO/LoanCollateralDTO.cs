using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO
{
    public class LoanCollateralDTO
    {
        [Required]
        public string LoanId { get; set; }

        [Required]
        public string CollateralId { get; set; }
    }
}
