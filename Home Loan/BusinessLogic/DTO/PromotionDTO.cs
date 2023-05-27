using Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.DTO
{
   public class PromotionDTO
    {
        public string PromotionId { get; set; }
        public bool Active { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public string Message { get; set; }
        public PromotionTypes Type { get; set; }
    }
}
