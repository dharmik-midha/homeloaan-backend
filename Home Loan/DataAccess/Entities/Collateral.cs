using Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Entities
{
    public class Collateral
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        public string OwnerEmail { get; set; }

        [Required]
        [Range(100000, 100000000, ErrorMessage = "Collateral value should be in range 1Lac to 10Cr")]
        public float CollateralValue { get; set; }

        [Required]
        public CollateralTypes CollateralType { get; set; }

        [Required]
        [Range(0, 100)]
        public int OwnShare { get; set; }

    }
}
