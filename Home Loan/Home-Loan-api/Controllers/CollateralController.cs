using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.DTO;
using DataAccess.Entities;
using Home_Loan_api.Models;
using Logging.NLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Home_Loan_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "User")]
    public class CollateralController : ControllerBase
    {
        #region Private Variables
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICollateralAppService _collateralService;
        private const string loggerPrefix = nameof(CollateralController);
        #endregion

        #region ctor
        public CollateralController(
            ILogger logger,
            IMapper mapper,
            ICollateralAppService collateralServices)
        {
            _logger = logger;
            _mapper = mapper;
            _collateralService = collateralServices;
        }
        #endregion

        #region GetALLCollateral
        /// <summary>
        /// All Collaterals
        /// </summary>
        /// <returns>Action Result</returns>
        /// <response code="200">Get All Collateral Successfully</response>
        /// <response code="400">Bad Request</response>

        [HttpGet]
        public ActionResult<IList<Collateral>> GetAllCollateral()
        {
            const string localLoggerPrefix = nameof(GetAllCollateral);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started.");
            
            ResultDTO<IList<Collateral>> result = _collateralService.GetAllCollateral(userEmail);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|completed : {result.StatusCode} : {result.Message}.");
            
            if (result.Success)
            {
                return new ActionResult<IList<Collateral>>(result.Data);
            }
            
            return StatusCode(result.StatusCode, result.Message);

        }
        #endregion

        #region AddCollateral
        /// <summary>
        /// Add Collateral
        /// </summary>
        ///<remarks>
        /// Sample request:
        ///
        ///     POST /Collateral
        ///     {
        ///         "collateralValue": 100000000,
        ///         "collateralType": 0,
        ///         "ownShare": 100
        ///     }
        ///
        /// </remarks>
        /// <param name="newCollateralModal"></param>
        /// <returns>Action Result</returns>
        /// <response code="201">Collateral Added Successfully</response>
        /// <response code="400">Bad Request</response>

        [HttpPost]
        public ObjectResult AddCollateral(NewCollateralModal newCollateralModal)
        {
            //put useremail
            const string localLoggerPrefix = nameof(AddCollateral);
            CollateralDTO collateral = _mapper.Map<NewCollateralModal, CollateralDTO>(newCollateralModal);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started.");
            
            collateral.OwnerEmail = userEmail;
            ResultDTO result = _collateralService.AddCollateral(collateral);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|completed : {result.StatusCode} : {result.Message}.");
            
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Update Collateral
        /// <summary>
        /// Update Collateral
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Collateral
        ///    {
        ///         "Id = "string" ,
        ///         "ownerName": "string",
        ///         "collateralValue": 100000000,
        ///         "collateralType": 0,
        ///         "ownShare": 100
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="newCollateralModal"></param>
        /// <returns>Object Result</returns>
        /// <response code="201">Collateral Updated Successfully</response>
        /// <response code="400">Bad Request</response>


        [HttpPut]
        public ObjectResult UpdateCollateral(string id, [FromBody] NewCollateralModal newCollateralModal)
        {
            //put useremail
            const string localLoggerPrefix = nameof(UpdateCollateral);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            
            CollateralDTO collateral = _mapper.Map<NewCollateralModal, CollateralDTO>(newCollateralModal);
            ResultDTO result = _collateralService.UpdateCollateral(id, collateral);
            
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|Collateral ID : {id}|completed : {result.StatusCode} : {result.Message}.");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Collateral
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Collateral
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Object Result</returns>
        /// <response code="201">Collateral deleted Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpDelete]
        public ObjectResult DeleteCollateral(string id)
        {
            //put email and check if email owns the collateral
            const string localLoggerPrefix = nameof(DeleteCollateral);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            
            ResultDTO result = _collateralService.DeleteCollateral(id);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|Collateral ID : {id}|completed : {result.StatusCode} : {result.Message}.");
            
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion
    }
}
