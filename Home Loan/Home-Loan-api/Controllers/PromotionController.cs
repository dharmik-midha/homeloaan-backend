using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.DTO;
using Home_Loan_api.Models;
using Logging.NLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Home_Loan_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PromotionController : ControllerBase
    {
        #region Private Variables
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPromotionAppServices _PromotionAppService;
        private const string loggerPrefix = nameof(PromotionController);
        #endregion

        #region ctor
        public PromotionController( ILogger logger,IMapper mapper, IPromotionAppServices PromotionAppServices)
        {
            _logger = logger;
            _mapper = mapper;
            _PromotionAppService = PromotionAppServices;
        }
        #endregion

        #region GetPromotion
        /// <summary>
        /// Get Promotion
        /// </summary>
        ///  <remarks>
        /// Sample request:
        ///
        ///     GET /Promotion
        ///     {   
        ///     }
        ///  </remarks>   
        /// <returns>Action Result</returns>
        /// <response code="200">Retrieve Promotion Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<PromotionModal> GetPromotion()
        {
            const string localLoggerPrefix = nameof(GetPromotion);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            var promotionResult = _PromotionAppService.GetPromotion();
            
            if (promotionResult.Success)
            {
                PromotionModal result = _mapper.Map<PromotionDTO, PromotionModal>(promotionResult.Data);
                return new ActionResult<PromotionModal>(result);
            }

            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|completed : {promotionResult.StatusCode} : {promotionResult.Message}.");
            return StatusCode(promotionResult.StatusCode, promotionResult.Message);
        }
        #endregion

        #region New Promotion
        /// <summary>
        /// New Promotion
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Promotion
        ///     {
        ///        "start_date": "2023-04-11",
        ///        "end_date": "2023-04-11",
        ///        "message": "string",
        ///        "type": 0
        ///                 
        ///     }
        ///  </remarks>   
        /// <param name="promotionModal"></param>
        /// <returns>Action Result</returns>
        /// <response code="201">New Promotion added Successfully</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Authorize(Roles ="Advisor")]
        public ActionResult AddNewPromotion(PromotionModal promotionModal)
        {
            const string localLoggerPrefix = nameof(AddNewPromotion);
            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|started.");
            
            var promotionDto = _mapper.Map<PromotionModal, PromotionDTO>(promotionModal);
            ResultDTO result = _PromotionAppService.AddPromotion(promotionDto);

            _logger.LogTrace($"{loggerPrefix}|{localLoggerPrefix}|completed : {result.StatusCode} : {result.Message}.");
            return StatusCode(result.StatusCode, result.Message);
        }
        #endregion
    }
}
