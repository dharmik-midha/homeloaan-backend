using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.DTO;
using Home_Loan_api.Models;
using Logging.NLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Home_Loan_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class StaticController : ControllerBase
    {
        #region Private Variables
        private readonly ILogger _logger;
        private readonly IStaticAppService _staticAppService;
        private readonly IMapper _mapper;
        private const string loggerPrefix = nameof(StaticController);
        #endregion

        #region ctor
        public StaticController(
            ILogger logger,
            IStaticAppService staticAppService,
            IMapper mapper)
        {
            _logger = logger;
            _staticAppService = staticAppService;
            _mapper = mapper;
        }
        #endregion

        //Country

        #region Get Countries
        /// <summary>
        /// Get All Countries
        /// </summary>
        /// <returns>Action Result</returns>
        /// <response code="200">Retrieve all Countries Successfully</response>
        /// <response code="400">Bad Request</response>

        [HttpGet]
        [AllowAnonymous]
        [Route("country")]
        public ActionResult<IList<CountryDTO>> GetCountries()
        {
            const string localLoggerPrefix = nameof(GetCountries);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            ResultDTO<IList<CountryDTO>> result = _staticAppService.GetCountries();

            if (result.Success)
            {
                return new ActionResult<IList<CountryDTO>>(result.Data);
            }

            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|completed : {result.StatusCode} : {result.Message}.");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Add Country
        /// <summary>
        /// Adds a new country
        /// </summary>
        /// <param name="newCountry">The data for the new country.</param>
        /// <returns>An ActionResult containing the result of the operation.</returns>
        [HttpPost]
        [Authorize(Roles = "Advisor")]
        [Route("country")]
        public ActionResult AddCountry(CountryModal newCountry)
        {
            const string localLoggerPrefix = nameof(AddCountry);
            _logger.LogError($"{loggerPrefix}|{localLoggerPrefix}|started");

            // Map the incoming data to a DTO.
            var country = _mapper.Map<CountryModal, CountryDTO>(newCountry);

            // Call the application service to add the new country.
            var result = _staticAppService.AddCountry(country);

            _logger.LogError($"{loggerPrefix}|{localLoggerPrefix}|{result.Message}");
            return StatusCode(result.StatusCode, result.Message);
        }

        #endregion

        #region Get Country
        /// <summary>
        /// Get a single country by id
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("country/{countryId}")]
        public ActionResult<CountryDTO> GetCountry(string countryId)
        {
            const string localLoggerPrefix = nameof(GetCountry);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            ResultDTO<CountryDTO> result = _staticAppService.GetCountry(countryId);

            if (result.Success)
            {
                return new ActionResult<CountryDTO>(result.Data);
            }

            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|completed : {result.StatusCode} : {result.Message}.");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Update Country
        /// <summary>
        /// update a country
        /// </summary>
        /// <param name="id"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        [Authorize(Roles = "Advisor")]
        [HttpPut]
        [Route("country/{id}")]
        public IActionResult UpdateCountry(string id, CountryModal country)
        {
            const string localLoggerPrefix = nameof(UpdateCountry);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            CountryDTO countrydto = _mapper.Map<CountryModal, CountryDTO>(country);
            countrydto.CountryId = id;
            ResultDTO result = _staticAppService.UpdateCountry(countrydto);

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Delete Country
        /// <summary>
        /// delete a country
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Advisor")]
        [HttpDelete]
        [Route("country/{id}")]
        public IActionResult DeleteCountry(string id)
        {
            const string localLoggerPrefix = nameof(DeleteCountry);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");

            ResultDTO result = _staticAppService.DeleteCountry(id);

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        //state

        #region Get States
        /// <summary>
        /// Get all States in a country
        /// </summary>
        /// <param name="countryId"></param>
        /// 
        /// <returns>Action Result</returns>
        /// <response code="200">Retrieve all State Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [AllowAnonymous]
        [Route("country/{countryId}/state")]
        public ActionResult<IList<StateDTO>> GetStates(string countryId)
        {
            const string localLoggerPrefix = nameof(GetStates);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            ResultDTO<IList<StateDTO>> result = _staticAppService.GetStates(countryId);

            if (result.Success)
            {
                return new ActionResult<IList<StateDTO>>(result.Data);
            }

            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|Country id : {countryId}|completed : {result.StatusCode} : {result.Message}.");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Add State
        /// <summary>
        /// add state in a country
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Advisor")]
        [Route("country/{countryId}/state")]
        public ActionResult AddState(string countryId, StateModal newState)
        {
            const string localLoggerPrefix = nameof(AddState);
            _logger.LogError($"{loggerPrefix}|{localLoggerPrefix}|started");

            // Map the incoming data to a DTO.
            var state = _mapper.Map<StateModal, StateDTO>(newState);
            state.CountryId = countryId;

            // Call the application service to add the new state.
            var result = _staticAppService.AddState(state);

            _logger.LogError($"{loggerPrefix}|{localLoggerPrefix}|{result.Message}");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region GetState
        /// <summary>
        /// get state by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("state/{id}")]
        public ActionResult<StateDTO> GetState(string id)
        {
            const string localLoggerPrefix = nameof(GetState);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            ResultDTO<StateDTO> result = _staticAppService.GetState(id);

            if (result.Success)
            {
                _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|completed : {result.StatusCode} : {result.Message}.");

                return new ActionResult<StateDTO>(result.Data);
            }

            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|completed : {result.StatusCode} : {result.Message}.");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Update State
        /// <summary>
        /// update a state
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Advisor")]
        [Route("state/{id}")]
        public IActionResult UpdateState(string id, StateModal state)
        {
            const string localLoggerPrefix = nameof(UpdateState);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            StateDTO stateDto = _mapper.Map<StateModal, StateDTO>(state);
            stateDto.StateId = id;
            ResultDTO result = _staticAppService.UpdateState(stateDto);

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Delete State
        /// <summary>
        /// delete a state
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Advisor")]
        [Route("state/{id}")]
        public IActionResult DeleteState(string id)
        {
            const string localLoggerPrefix = nameof(DeleteState);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");

            ResultDTO result = _staticAppService.DeleteState(id);

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        //city

        #region Get Cities
        /// <summary>
        /// Get all cities in a state
        /// </summary>  
        /// <param name="stateId"></param>
        /// 
        /// <returns>Action Result</returns>
        /// <response code="200">Retrieve all Cities Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [AllowAnonymous]
        [Route("state/{stateId}/city")]
        public ActionResult<IList<CityDTO>> GetCities(string stateId)
        {
            const string localLoggerPrefix = nameof(GetCities);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            ResultDTO<IList<CityDTO>> result = _staticAppService.GetCities(stateId);

            if (result.Success)
            {
                return new ActionResult<IList<CityDTO>>(result.Data);
            }

            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|State id : {stateId}|completed : {result.StatusCode} : {result.Message}.");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Add City
        /// <summary>
        /// add a city in a state
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="newCity"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Advisor")]
        [Route("state/{stateId}/city")]
        public ActionResult AddCity(string stateId, CityModal newCity)
        {
            const string localLoggerPrefix = nameof(AddCity);
            _logger.LogError($"{loggerPrefix}|{localLoggerPrefix}|started");

            // Map the incoming data to a DTO.
            var city = _mapper.Map<CityModal, CityDTO>(newCity);
            city.StateId = stateId;

            // Call the application service to add the new city.
            var result = _staticAppService.AddCity(city);

            _logger.LogError($"{loggerPrefix}|{localLoggerPrefix}|{result.Message}");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Get City
        /// <summary>
        /// get city by city id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("city/{id}")]
        public ActionResult<CityDTO> GetCity(string id)
        {
            const string localLoggerPrefix = nameof(GetCity);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            ResultDTO<CityDTO> result = _staticAppService.GetCity(id);

            if (result.Success)
            {
                _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|completed : {result.StatusCode} : {result.Message}.");

                return new ActionResult<CityDTO>(result.Data);
            }

            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|completed : {result.StatusCode} : {result.Message}.");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Update City
        /// <summary>
        /// update city
        /// </summary>
        /// <param name="id"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Advisor")]
        [Route("city/{id}")]
        public IActionResult UpdateCity(string id, CityModal city)
        {
            const string localLoggerPrefix = nameof(UpdateCity);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            CityDTO cityDto = _mapper.Map<CityModal, CityDTO>(city);
            cityDto.CityId = id;
            ResultDTO result = _staticAppService.UpdateCity(cityDto);

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Delete City
        /// <summary>
        /// delete city
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Advisor")]
        [Route("city/{id}")]
        public IActionResult DeleteCity(string id)
        {
            const string localLoggerPrefix = nameof(DeleteCity);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");

            ResultDTO result = _staticAppService.DeleteCity(id);

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion
    }
}
