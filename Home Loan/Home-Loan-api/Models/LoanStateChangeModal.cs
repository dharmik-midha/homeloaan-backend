using System.ComponentModel.DataAnnotations;
using Constants.Enums;

namespace Home_Loan_api.Models
{
    public class LoanStateChangeModal
    {

        [Required]
        public LoanStates State { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }
    }
}
