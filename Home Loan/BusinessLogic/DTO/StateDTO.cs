using System;
using Constants.Enums;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTO
{
    public class StateDTO
    {
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string CountryId { get; set; }
    }
}
