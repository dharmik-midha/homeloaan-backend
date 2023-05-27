using System.ComponentModel.DataAnnotations;

namespace Home_Loan_api.Models
{
    public class StateModal
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string StateName { get; set; }
        [Required]
        [StringLength(3)]
        public string StateCode { get; set; }
    }
}