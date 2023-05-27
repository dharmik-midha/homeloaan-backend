using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DTO
{
    public class CountryStateCityDTO
    {
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
    }
}
