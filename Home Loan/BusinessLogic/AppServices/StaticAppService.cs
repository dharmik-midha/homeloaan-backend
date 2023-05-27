using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.Contracts;
using DataAccess.Entities;
using Logging.NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.AppServices
{
    public class StaticAppService : IStaticAppService
    {
        #region Private Variables

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private const string loggerPrefix = nameof(StaticAppService);

        #endregion Private Variables

        #region ctor

        public StaticAppService(IRepositoryManager repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion ctor



        #region Get Countries

        /// <summary>
        /// Retrieves a list of all countries from the database.
        /// </summary>
        /// <returns>A ResultDTO object containing a list of CountryDTO objects if successful, or an error message if not.</returns>
        public ResultDTO<IList<CountryDTO>> GetCountries()
        {
            const string localLoggerPrefix = nameof(GetCountries);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");

            // Retrieve list of all countries from repository
            List<Country> availableCountries = _repository.Country.FindAll(false).ToList();

            // Map the Country entities to CountryDTO objects
            IList<CountryDTO> country = _mapper.Map<IList<Country>, IList<CountryDTO>>(availableCountries);

            // Log the success message
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|successfully retrieved {country.Count} countries");

            // Return a success ResultDTO object with the list of CountryDTO objects
            return new ResultDTO<IList<CountryDTO>>(true, 200, "", country);
        }

        #endregion Get countries

        #region Add Country

        /// <summary>
        /// Adds a new country to the database.
        /// </summary>
        /// <param name="countrymodel">The DTO containing the country details to add.</param>
        /// <returns>A ResultDTO object indicating whether the operation was successful or not.</returns>
        public ResultDTO AddCountry(CountryDTO countrymodel)
        {
            const string localLoggerPrefix = nameof(AddCountry);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");

            countrymodel.CountryCode = countrymodel.CountryCode.ToUpper();

            Country availableCountry = _repository.Country
                .FindByCondition(x => x.CountryCode == countrymodel.CountryCode, false)
                .FirstOrDefault();
            if (availableCountry != null)
            {
                return new ResultDTO(false,400,$"Country with country code {countrymodel.CountryCode} already exists");
            }
            Country country = new Country()
            {
                CountryId = Guid.NewGuid().ToString(),
                CountryCode = countrymodel.CountryCode,
                CountryName = countrymodel.CountryName
            };
            _repository.Country.Create(country);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Country added with ID : {country.CountryId}");
            _repository.Save();
            return new ResultDTO(false, 201, $"Country created with Id : {country.CountryId}");
        }

        #endregion Add Country / Check Country

        #region GetCountry
        /// <summary>
        /// Retrieves a single country by its ID.
        /// </summary>
        /// <param name="countryId">The ID of the country to retrieve.</param>
        /// <returns>A <see cref="ResultDTO{T}"/> containing either the retrieved <see cref="CountryDTO"/> object or an error message.</returns>
        public ResultDTO<CountryDTO> GetCountry(string countryId)
        {
            const string localLoggerPrefix = nameof(GetCountry);

            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");

            var country = _repository.Country.GetByKey(countryId);

            if (country == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Country not found with ID : {countryId}");
                return new ResultDTO<CountryDTO>(false, 404, "Country not found", null);
            }

            CountryDTO countryDTO = _mapper.Map<Country, CountryDTO>(country);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Country found with ID : {countryId}");
            return new ResultDTO<CountryDTO>(true, 200, "", countryDTO);
        }
        #endregion

        #region Update Country
        /// <summary>
        /// Updates an existing country in the repository.
        /// </summary>
        /// <param name="countrymodel">The CountryDTO object containing the updated country information.</param>
        /// <returns>A ResultDTO object indicating the success or failure of the update operation.</returns>
        public ResultDTO UpdateCountry(CountryDTO countrymodel)
        {
            const string localLoggerPrefix = nameof(UpdateCountry);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");

            // Retrieve the existing country from the repository.
            Country availableCountry = _repository.Country.GetByKey(countrymodel.CountryId);

            if (availableCountry == null)
            {
                // If the country is not found, return an error result.
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Country not found with ID : {countrymodel.CountryId}");
                return new ResultDTO(false, 404, "Country not found");
            }

            availableCountry.CountryCode = countrymodel.CountryCode.ToUpper();
            availableCountry.CountryName = countrymodel.CountryName;

            // Update the country in the repository.
            _repository.Country.Update(availableCountry);

            // Save the changes to the repository.
            _repository.Save();

            // Return a success result.
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Country updated with ID : {availableCountry.CountryId}");
            return new ResultDTO(true, 204, "");
        }

        #endregion

        #region Delete Country
        /// <summary>
        /// Deletes a country from the database.
        /// </summary>
        /// <param name="id">The ID of the country to delete.</param>
        /// <returns>A <see cref="ResultDTO"/> object indicating whether the country was deleted successfully.</returns>
        public ResultDTO DeleteCountry(string id)
        {
            const string localLoggerPrefix = nameof(DeleteCountry);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");

            // Retrieve the country to be deleted
            Country availableCountry = _repository.Country.GetByKey(id);

            // Check if the country exists
            if (availableCountry == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Country with ID: {id} not found");
                return new ResultDTO(false, 404, "Country Not found");
            }

            // Retrieve all states associated with the country
            var states = GetStates(id).Data;

            // Delete each state in country
            foreach (var state in states)
            {
                DeleteState(state.StateId);
            }

            // Delete the country
            _repository.Country.Delete(availableCountry);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Country Deleted with ID : {id}");
            _repository.Save();

            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|DeleteCountry function completed successfully for ID: {id}");
            return new ResultDTO(true, 204, "");
        }
        #endregion



        #region Get States
        /// <summary>
        /// Retrieves a list of states for a given country ID.
        /// </summary>
        /// <param name="countryId">The ID of the country for which to retrieve states.</param>
        /// <returns>A <see cref="ResultDTO{T}"/> object containing a list of <see cref="StateDTO"/> objects representing the states for the specified country.</returns>
        public ResultDTO<IList<StateDTO>> GetStates(string countryId)
        {
            const string localLoggerPrefix = nameof(GetStates);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Function started with country ID : {countryId}");

            // Retrieve list of states for the given country ID
            var availableStates = _repository.State.FindByCondition(x => x.CountryId == countryId, false).ToList();

            // Map the list of states to a list of StateDTO objects
            IList<StateDTO> states = _mapper.Map<IEnumerable<State>, IList<StateDTO>>(availableStates);

            // Log the country ID and return the list of states
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Function completed successfully for country ID : {countryId}");
            return new ResultDTO<IList<StateDTO>>(true, 200, "", states);
        }
        #endregion Get State

        #region Add State

        /// <summary>
        /// Adds a new state to the database.
        /// </summary>
        /// <param name="statemodel">The <see cref="StateDTO"/> object containing the state information to add.</param>
        /// <returns>A <see cref="ResultDTO"/> object indicating whether the state was added successfully.</returns>
        public ResultDTO AddState(StateDTO statemodel)
        {
            const string localLoggerPrefix = nameof(AddState);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Function started");

            // Convert state code to uppercase
            statemodel.StateCode = statemodel.StateCode.ToUpper();

            // Check if state already exists for the given country
            State availableState = _repository.State
                .FindByCondition(x =>
                    x.CountryId == statemodel.CountryId &&
                    x.StateCode == statemodel.StateCode
                    , false)
                .FirstOrDefault();

            // If state already exists, return error response
            if (availableState != null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|State already present with code: {statemodel.StateCode} in country with ID: {statemodel.CountryId}");
                return new ResultDTO(false, 400, $"State already present with code: {statemodel.StateCode} in country");
            }

            // Create new State object
            State state = new State()
            {
                StateId = Guid.NewGuid().ToString(),
                StateCode = statemodel.StateCode,
                StateName = statemodel.StateName,
                CountryId = statemodel.CountryId
            };

            // Add new state to repository and save changes
            _repository.State.Create(state);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|State added with ID : {state.StateId}");
            _repository.Save();

            // Return success response
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Function completed successfully for state with ID: {state.StateId}");
            return new ResultDTO(true, 200, $"State added with ID: {state.StateId}");
        }
        #endregion Add State

        #region GetState
        /// <summary>
        /// Get a single state by stateId.
        /// </summary>
        /// <param name="stateId">The Id of the state to retrieve.</param>
        /// <returns>A <see cref="ResultDTO"/> containing the retrieved <see cref="StateDTO"/> object or an error message if the state is not found.</returns>
        public ResultDTO<StateDTO> GetState(string stateId)
        {
            const string localLoggerPrefix = nameof(GetState);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");

            var state = _repository.State.GetByKey(stateId);
            if (state == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|state not found with id: {stateId}");
                return new ResultDTO<StateDTO>(false, 404, $"State not found with ID: {stateId}", null);
            }
            StateDTO stateDTO = _mapper.Map<State, StateDTO>(state);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|state retrieved with ID : {stateDTO.StateId}");
            return new ResultDTO<StateDTO>(true, 200, "", stateDTO);
        }
        #endregion

        #region Update State
        /// <summary>
        /// Updates a State entity in the database based on the StateDTO input model
        /// </summary>
        /// <param name="statemodel">The StateDTO model to update</param>
        /// <returns>A ResultDTO indicating success or failure</returns>
        public ResultDTO UpdateState(StateDTO statemodel)
        {
            const string localLoggerPrefix = nameof(UpdateState);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");

            // Check if the state exists in the database
            State availableState = _repository.State.GetByKey(statemodel.StateId);
            if (availableState == null)
            {
                _logger.LogError($"{loggerPrefix}|{localLoggerPrefix}|State with ID : {statemodel.StateId} not found");
                return new ResultDTO(false, 404, "State Not found");
            }

            // Convert the state code to uppercase
            availableState.StateCode = statemodel.StateCode.ToUpper();
            availableState.StateName = statemodel.StateName;
            
            
            _repository.State.Update(availableState);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|State Updated with ID : {availableState.StateId}");

            // Save the changes to the database and return a success ResultDTO
            _repository.Save();
            return new ResultDTO(true, 204, "");
        }

        #endregion

        #region Delete State
        /// <summary>
        /// Deletes a state and all its associated cities from the database.
        /// </summary>
        /// <param name="id">The ID of the state to delete.</param>
        /// <returns>A <see cref="ResultDTO"/> indicating whether the operation was successful.</returns>
        public ResultDTO DeleteState(string id)
        {
            const string localLoggerPrefix = nameof(DeleteState);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");

            State availableState = _repository.State.GetByKey(id);
            if (availableState == null)
            {
                _logger.LogError($"{loggerPrefix}|{localLoggerPrefix}|State not found with ID : {id}");
                return new ResultDTO(false, 404, "State not found");
            }

            // delete all cities in this state
            var cities = GetCities(id).Data;
            foreach (var city in cities)
            {
                DeleteCity(city.CityId);
            }

            //delete the state
            _repository.State.Delete(availableState);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|State Deleted with ID : {id}");
            _repository.Save();

            return new ResultDTO(true, 204, "");
        }

        #endregion



        #region Get Cities
        /// <summary>
        /// Retrieves all cities belonging to a state.
        /// </summary>
        /// <param name="stateId">The ID of the state to retrieve cities for.</param>
        /// <returns>A ResultDTO object containing a list of CityDTO objects.</returns>
        public ResultDTO<IList<CityDTO>> GetCities(string stateId)
        {
            const string localLoggerPrefix = nameof(GetCities);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Started with state ID: {stateId}");

            // Retrieve all cities belonging to the specified state.
            var availableCities = _repository.City.FindByCondition(x => x.StateId == stateId, false).ToList();

            // Map the retrieved cities to CityDTO objects.
            IList<CityDTO> cities = _mapper.Map<IEnumerable<City>, IList<CityDTO>>(availableCities);

            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Returned {cities.Count()} cities for state ID: {stateId}");

            // Return a ResultDTO object containing the list of CityDTO objects.
            return new ResultDTO<IList<CityDTO>>(true, 200, "", cities);
        }

        #endregion Get Cities

        #region Add City
        /// <summary>
        /// Adds a new city to the database.
        /// </summary>
        /// <param name="citymodel">The city to be added.</param>
        /// <returns>A result indicating whether the operation was successful and the new city's ID if applicable.</returns>
        public ResultDTO AddCity(CityDTO citymodel)
        {
            const string localLoggerPrefix = nameof(AddCity);

            // Convert the city code to uppercase for consistency.
            citymodel.CityCode = citymodel.CityCode.ToUpper();

            // Check if a city with the same code and state already exists in the database.
            City availableCity = _repository.City.FindByCondition(x =>
                x.CityCode == citymodel.CityCode &&
                x.StateId == citymodel.StateId, false)
                .FirstOrDefault();

            if (availableCity != null)
            {
                // Return an error result if the city already exists.
                _logger.LogWarning($"{loggerPrefix}|{localLoggerPrefix}|Failed to add city with city code '{citymodel.CityCode}' because it already exists for state '{citymodel.StateId}'.");
                return new ResultDTO(false, 400, $"A city with city code '{citymodel.CityCode}' already exists for state '{citymodel.StateId}'.");
            }

            // Create a new City object and add it to the repository.
            City city = new City()
            {
                CityId = Guid.NewGuid().ToString(),
                CityCode = citymodel.CityCode,
                CityName = citymodel.CityName,
                StateId = citymodel.StateId
            };

            _repository.City.Create(city);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Added city '{citymodel.CityName}' with ID '{city.CityId}' for state '{citymodel.StateId}' to the database.");
            _repository.Save();

            // Return a success result with the new city's ID.
            return new ResultDTO(true, 200, $"Added city '{citymodel.CityName}' with ID '{city.CityId}' for state '{citymodel.StateId}' to the database.");
        }

        #endregion Add City

        #region Get City
        /// <summary>
        /// Retrieves a city by its ID.
        /// </summary>
        /// <param name="cityId">The ID of the city to retrieve.</param>
        /// <returns>A <see cref="ResultDTO"/> containing the retrieved city if successful, or an error message if unsuccessful.</returns>
        public ResultDTO<CityDTO> GetCity(string cityId)
        {
            const string localLoggerPrefix = nameof(GetCity);

            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Started with city ID: {cityId}");

            City availableCity = _repository.City.GetByKey(cityId);
            if (availableCity == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|City not found with ID: {cityId}");
                return new ResultDTO<CityDTO>(false, 404, $"City not found with ID: {cityId}",null);
            }

            CityDTO cityDTO = _mapper.Map<City, CityDTO>(availableCity);

            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Returned city with ID: {cityId}");
            return new ResultDTO<CityDTO>(true, 200, "", cityDTO);
        }

        #endregion

        #region Edit City

        /// <summary>
        /// Updates an existing city.
        /// </summary>
        /// <param name="citymodel">The city to be updated.</param>
        /// <returns>A <see cref="ResultDTO"/> indicating whether the update was successful.</returns>
        public ResultDTO UpdateCity(CityDTO citymodel)
        {
            const string localLoggerPrefix = nameof(UpdateCity);

            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Started with city ID: {citymodel.CityId}");

            // Check if the city to be updated exists
            City availableCity = _repository.City.GetByKey(citymodel.CityId);
            if (availableCity == null)
            {
                _logger.LogError($"{loggerPrefix}|{localLoggerPrefix}|City with ID {citymodel.CityId} not found.");
                return new ResultDTO(false, 404, "City not found.");
            }

            // Update the city with the new data
            availableCity.CityCode = citymodel.CityCode.ToUpper();
            availableCity.CityName = citymodel.CityName;

            _repository.City.Update(availableCity);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|City updated with ID: {availableCity.CityId}");

            _repository.Save();
            return new ResultDTO(true, 204, "");
        }

        #endregion

        #region Delete City
        /// <summary>
        /// Deletes the specified city by ID.
        /// </summary>
        /// <param name="id">The ID of the city to be deleted.</param>
        /// <returns>A ResultDTO indicating whether the operation was successful.</returns>
        public ResultDTO DeleteCity(string id)
        {
            const string localLoggerPrefix = nameof(DeleteCity);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");

            City availableCity = _repository.City.GetByKey(id);

            // If city is not found, return error
            if (availableCity == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|City not found with ID : {id}");
                return new ResultDTO(false, 404, "City not found");
            }

            _repository.City.Delete(availableCity);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|City Deleted with ID : {id}");
            _repository.Save();

            // Return success result
            return new ResultDTO(true, 204, "");
        }

        #endregion



        #region validate city state country

        /// <summary>
        ///
        /// Validate city state and country
        /// </summary>
        /// <param name="cityCode"></param>
        /// <param name="stateCode"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public ResultDTO IsValidCityStateCountry(string cityCode, string stateCode, string countryCode)
        {
            const string localLoggerPrefix = nameof(IsValidCityStateCountry);
            countryCode = countryCode.ToUpper();
            stateCode = stateCode.ToUpper();
            cityCode = cityCode.ToUpper();

            var countries = GetCountries().Data
                .Where(x => x.CountryCode == countryCode);
            if (countries.Count() == 0)
            {
                return new ResultDTO(false, 400, "Country code is Invalid");
            }
            var countryId = countries.FirstOrDefault().CountryId;
            var states = GetStates(countryId).Data
                .Where(X => X.StateCode == stateCode);
            if (states.Count() == 0)
            {
                return new ResultDTO(false, 400, "State code is Invalid");
            }
            var stateId = states.FirstOrDefault().StateId;
            var cities = GetCities(stateId).Data.ToList()
                .Where(x => x.CityCode == cityCode);
            if (cities.Count() == 0)
            {
                return new ResultDTO(false, 400, "City code is Invalid");
            }
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Country State City Validated");
            return new ResultDTO(true, 200, "Country State City Validated");
        }
        #endregion
    }
}