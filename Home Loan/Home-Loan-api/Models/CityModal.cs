using System.ComponentModel.DataAnnotations;

namespace Home_Loan_api.Models
{
    public class CityModal
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string CityName { get; set; }
        [Required]
        [StringLength(3)]
        public string CityCode { get; set; }
    }
}