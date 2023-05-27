using System;
using Constants.Enums;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTO
{
    public class CountryDTO
    {
        public string CountryId { get; set; }        
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

    }
}
