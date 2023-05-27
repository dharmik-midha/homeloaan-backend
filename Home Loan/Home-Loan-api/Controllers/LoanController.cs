using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.DTO;
using Logging.NLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Home_Loan_api.Models;

namespace Home_Loan_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LoanController : ControllerBase
    {
        #region Private Variables
        private readonly ILogger _logger;
        private readonly ILoanAppServices _loanAppService;
        private readonly ILoanStateAppService _loanStateAppService;
        private readonly IMapper _mapper;
        private const string loggerPrefix = nameof(LoanController);
        #endregion

        #region ctor
        public LoanController(
            ILogger logger,
            ILoanAppServices loanAppServices,
            ILoanStateAppService loanStateAppService,
            IMapper mapper
            )
        {
            _logger = logger;
            _loanAppService = loanAppServices;
            _loanStateAppService = loanStateAppService;
            _mapper = mapper;
        }
        #endregion

        #region NewLoan
        /// <summary>
        /// New Loan
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Loan
        ///    {
        ///       "propertyAddress": "string",
        ///       "propertySize": 100000000,
        ///       "propertyCost": 100000000,
        ///       "propertyRegistrationCost": 100000000,
        ///       "monthlyFamilyIncome": 100000000,
        ///       "otherIncome": 100000000,
        ///       "loanAmount": 100000000,
        ///       "loanDuration": 240
        ///    }
        /// </remarks>   
        /// <param name="loan"></param>
        /// <returns>Action Result</returns>
        /// <response code="200">New Loan Created Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult NewLoan(NewLoanModal loan)
        {
            const string localLoggerPrefix = nameof(NewLoan);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started.");

            var loanDto = _mapper.Map<NewLoanModal, LoanDTO>(loan);
            loanDto.UserEmail = userEmail;
            ResultDTO result = _loanAppService.CreateNewLoan(loanDto);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|completed : {result.StatusCode} : {result.Message}.");

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Apply Loan
        /// <summary>
        /// Apply Loan
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Loan
        ///    {
        ///    }
        /// </remarks>   
        /// <param name="loanId"></param>
        /// <returns>Action Result</returns>
        /// <response code="200">Loan Applied Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("{loanId}")]
        [Authorize(Roles = "User")]
        public ActionResult ApplyLoan(string loanId)
        {
            const string localLoggerPrefix = nameof(ApplyLoan);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|loan id : {loanId}|started.");

            ResultDTO result = _loanAppService.ApplyLoan(userEmail, loanId);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|loan id : {loanId}|completed : {result.StatusCode} : {result.Message}.");

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region EditLoan
        /// <summary>
        /// Edit Loan
        /// </summary>
        ///  <remarks>
        /// Sample request:
        ///
        ///     PATCH /Loan
        ///    {
        ///       "propertyAddress": "string",
        ///       "propertySize": 100000000,
        ///       "propertyCost": 100000000,
        ///       "propertyRegistrationCost": 100000000,
        ///       "monthlyFamilyIncome": 100000000,
        ///       "otherIncome": 100000000,
        ///       "loanAmount": 100000000,
        ///       "loanDuration": 240
        ///       
        ///    }
        /// </remarks>   
        /// <param name="editvalues"></param>
        /// <returns>Action Result</returns>
        /// <response code="200">Loan Editted Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpPatch]
        [Route("{loanId}")]
        [Authorize(Roles = "User")]
        public IActionResult EditloanValues(string loanId, ModifyLoanModal editvalues)
        {
            const string localLoggerPrefix = nameof(EditloanValues);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|loan id : {loanId}|started");

            var loanDto = _mapper.Map<ModifyLoanModal, LoanDTO>(editvalues);
            loanDto.UserEmail = userEmail;
            loanDto.LoanId = loanId;

            ResultDTO result = _loanAppService.EditLoanValues(loanDto);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|loan id : {loanId}|completed : {result.StatusCode} : {result.Message}.");

            return StatusCode(result.StatusCode, result.Message);
        }

        #endregion

        #region GetLoan
        /// <summary>
        /// get a loan by loanId
        /// </summary>
        ///  <remarks>
        /// Sample request:
        ///
        ///     GET /LoanId
        ///    {
        ///    }
        /// </remarks>   
        /// <returns>Action Result</returns>
        /// <response code="200"> Retrieve Loan Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("{LoanId}")]
        public ActionResult<LoanDTO> GetLoan(string LoanId)
        {
            const string localLoggerPrefix = nameof(GetLoan);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started.");

            ResultDTO<LoanDTO> result = _loanAppService.GetUserLoan(userEmail, LoanId);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|loan id : {LoanId}|completed : {result.StatusCode} : {result.Message}.");

            if (result.Success)
            {
                return new ActionResult<LoanDTO>(result.Data);
            }

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region GetLoans
        /// <summary>
        /// Get User Loans
        /// </summary>
        ///   <remarks>
        /// Sample request:
        ///
        ///     GET /Loan
        ///    {
        ///    }
        /// </remarks>   
        /// <returns>Action Result</returns>
        /// <response code="200">Retrieve User loans Successfully</response>
        /// <response code="400">Bad Request</response>

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult<IList<LoanDTO>> GetLoans()
        {
            const string localLoggerPrefix = nameof(GetLoans);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started. ");

            ResultDTO<IList<LoanDTO>> result = _loanAppService.GetUserLoans(userEmail);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|completed : {result.StatusCode} : {result.Message}.");

            if (result.Success)
            {
                return new ActionResult<IList<LoanDTO>>(result.Data);
            }

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region GetAllLoans (Advisor)
        /// <summary>
        /// Get All Loans by Advisor
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Loan
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <returns>Action Result</returns>
        /// <response code="200">Retrieve All Loans by Advisor Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [Route("get-all")]
        [Authorize(Roles = "Advisor")]
        public ActionResult<IList<LoanViewDTO>> GetAllLoans()
        {
            const string localLoggerPrefix = nameof(GetAllLoans);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");

            ResultDTO<IList<LoanViewDTO>> result = _loanAppService.GetAllLoans();
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|completed : {result.StatusCode} : {result.Message}.");

            if (result.Success)
            {
                return new ActionResult<IList<LoanViewDTO>>(result.Data);
            }

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Add Collateral to Loan
        /// <summary>
        /// Add Collateral to Loan
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Add Collateral to Loan
        ///     {
        ///         "collateralId": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="loanCollateral"></param>
        /// <returns>Object Result</returns>
        /// <response code="201">Collateral Added Successfully</response>
        /// <response code="400">Bad Request</response>

        [HttpPost]
        [Route("{loanId}/collateral")]
        [Authorize(Roles = "User")]
        public ObjectResult AddCollateral(string loanId, AddLoanCollateralModal collateral)
        {
            const string localLoggerPrefix = nameof(AddCollateral);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started.");

            ResultDTO result = _loanAppService.AddCollateral(userEmail, loanId, collateral.CollateralId);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|Loan ID : {loanId}|Collateral ID : {collateral.CollateralId}|completed : {result.StatusCode} : {result.Message}.");

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Remove Collateral from Loan
        /// <summary>
        /// Remove Collateral from Loan
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE / Collateral
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <param name="loanCollateralId"></param>
        /// <returns>Object Result</returns>
        /// <response code="201">Collateral Removed Successfully</response>
        /// <response code="400">Bad Request</response>

        [HttpDelete]
        [Route("{loanId}/collateral/{collateralId}")]
        [Authorize(Roles = "User")]
        public ObjectResult RemoveCollateralFromLoan(string loanId, string collateralId)
        {
            const string localLoggerPrefix = nameof(RemoveCollateralFromLoan);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started.");

            ResultDTO result = _loanAppService.RemoveCollateralFromLoan(userEmail, loanId, collateralId);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|Loan ID : {loanId}|Collateral ID : {collateralId}|completed : {result.StatusCode} : {result.Message}.");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Get all collateral in Loan
        /// <summary>
        /// Get all Collateral in Loan
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET / Collateral 
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <param name="loanId"></param>
        /// <returns>Action Result</returns>
        /// <response code="200">Retrieve all collateral in Loan Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [Route("{loanId}/collateral")]
        [Authorize(Roles = "User")]
        public ActionResult<IList<CollateralDTO>> GetCollateralInLoan(string loanId)
        {
            const string localLoggerPrefix = nameof(GetCollateralInLoan);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");

            ResultDTO<IList<CollateralDTO>> result = _loanAppService.GetCollateralInLoan(loanId);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|Loan ID : {loanId}|completed : {result.StatusCode} : {result.Message} ");

            if (result.Success)
            {
                return new ActionResult<IList<CollateralDTO>>(result.Data);
            }

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Loan state change
        /// <summary>
        /// Loan State Change
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST / Loan State
        ///     {
        ///         "loanId": "string",
        ///         "state"= 0,
        ///         "notes": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="loanStateChange"></param>
        /// <returns>Object Result</returns>
        /// <response code="201">Loan State Change Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("{loanId}/state")]
        //[AllowAnonymous]
        [Authorize(Roles = "Advisor")]
        public ObjectResult LoanStateChange(string loanId, LoanStateChangeModal loanStateChange)
        {
            const string localLoggerPrefix = nameof(LoanStateChange);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started.");

            var loanStateDto = _mapper.Map<LoanStateChangeModal, LoanStateDTO>(loanStateChange);
            loanStateDto.UserID = userEmail;
            loanStateDto.LoanID = loanId;

            ResultDTO result = _loanStateAppService.ChangeLoanState(loanStateDto);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|Loan Id : {loanId}|completed : {result.StatusCode} : {result.Message}");

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion

        #region Get Loan State History
        /// <summary>
        /// Get Loan State History
        /// </summary>
        ///  <remarks>
        /// Sample request:
        ///
        ///     GET /LoanId
        ///    {
        ///    }
        /// </remarks>   
        /// <returns>Action Result</returns>
        /// <response code="200"> Retrieve Loan state Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("{LoanId}/state")]
        public ActionResult<IList<LoanStateDTO>> GetLoanStateHistory(string LoanId)
        {
            const string localLoggerPrefix = nameof(GetLoanStateHistory);
            var userEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|started.");

            ResultDTO<IList<LoanStateDTO>> result = _loanAppService.GetLoanStateHistory(userEmail, LoanId);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|email : {userEmail}|loan id : {LoanId}|completed : {result.StatusCode} : {result.Message}.");

            if (result.Success)
            {
                return new ActionResult<IList<LoanStateDTO>>(result.Data);
            }

            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion
    }
}
