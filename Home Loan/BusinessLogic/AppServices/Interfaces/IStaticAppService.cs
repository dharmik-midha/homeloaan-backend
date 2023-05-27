using BusinessLogic.DTO;
using System.Collections.Generic;

namespace BusinessLogic.AppServices
{
    public interface IStaticAppService
    {
        public ResultDTO<IList<CountryDTO>> GetCountries();
        public ResultDTO AddCountry(CountryDTO countrymodel);
        public ResultDTO<CountryDTO> GetCountry(string countryId);
        public ResultDTO UpdateCountry(CountryDTO countrymodel);
        public ResultDTO DeleteCountry(string id);

        public ResultDTO<IList<StateDTO>> GetStates(string countryId);
        public ResultDTO AddState(StateDTO statemodel);
        public ResultDTO<StateDTO> GetState(string stateId);
        public ResultDTO UpdateState(StateDTO statemodel);
        public ResultDTO DeleteState(string id);
        
        public ResultDTO<IList<CityDTO>> GetCities(string stateId);
        public ResultDTO AddCity(CityDTO citymodel);
        public ResultDTO<CityDTO> GetCity(string cityId);
        public ResultDTO UpdateCity(CityDTO citymodel);
        public ResultDTO DeleteCity(string id);

        public ResultDTO IsValidCityStateCountry(string cityCode, string stateCode, string countryCode);
        //public ResultDTO AddLocation(CountryStateCityDTO newLocation);
    }
}