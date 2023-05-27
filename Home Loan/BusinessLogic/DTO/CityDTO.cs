using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.DTO
{
    public class CityDTO
    {
        public string CityId { get; set; }       
        public string CityName { get; set; }  
        public string CityCode { get; set; } 
        public string StateId { get; set; }
    }
}
