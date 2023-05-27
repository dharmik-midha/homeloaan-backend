using System.ComponentModel.DataAnnotations;

namespace Home_Loan_api.Models
{
    public class CountryModal
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string CountryName { get; set; }
        [Required]
        [StringLength(3)]
        public string CountryCode { get; set; }
    }
}