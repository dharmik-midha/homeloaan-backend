using System;
using Constants.Enums;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTO
{
    public class CollateralDTO
    {
        public string Id { get; set; }
        public string OwnerEmail { get; set; }
        public float CollateralValue { get; set; }
        public CollateralTypes CollateralType { get; set; }
        public int OwnShare { get; set; }
    }
}
