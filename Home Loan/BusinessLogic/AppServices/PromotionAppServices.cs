using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using Logging.NLog;
using System;
using System.Linq;

namespace BusinessLogic.AppServices
{
    public class PromotionAppServices : IPromotionAppServices
    {
        #region Private Variables
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRepositoryManager _repository;
        private const string loggerPrefix = nameof(PromotionAppServices);
        #endregion

        #region ctor
        public PromotionAppServices(IMapper mapper, ILogger logger,IRepositoryManager repositoryManager)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repositoryManager;
        }
        #endregion

        #region AddPromotion
        /// <summary>
        /// Add Promotion
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        public ResultDTO AddPromotion(PromotionDTO pmodel)
        {
            const string localLoggerPrefix = nameof(AddPromotion);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Promotion ID : {pmodel.PromotionId}|started");
            Promotion activePromotion = _repository.Promotion.FindByCondition(x => x.Active == true, false).FirstOrDefault();
            if (activePromotion != null)
            {
                activePromotion.Active = false;
                _repository.Promotion.Update(activePromotion);
            }
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Add new Bank Promotion:");
            Promotion promotion = _mapper.Map<PromotionDTO, Promotion>(pmodel);
            promotion.Active = true;
            promotion.PromotionId = Guid.NewGuid().ToString();
            _repository.Promotion.Create(promotion);
            _repository.Save();
            _logger.LogInfo($"{loggerPrefix} | {localLoggerPrefix} : Bank Promotion Added with Id: {promotion.PromotionId}");
            
            return new ResultDTO(true,201, "Bank Promotion Added");

        }
        #endregion

        #region getpromotion
        /// <summary>
        /// Get promotion
        /// </summary>
        /// <returns></returns>
        public ResultDTO<PromotionDTO> GetPromotion()
        {
            const string localLoggerPrefix = nameof(GetPromotion);
            Promotion activePromotion = _repository.Promotion.FindByCondition(x => x.Active == true,false).FirstOrDefault();
            if (activePromotion == null || activePromotion.End_date < DateTime.Now)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|No Active Bank Promotion Found.");
                return new ResultDTO<PromotionDTO>(false,404,"No Active Bank Promotion",null);
            }
            PromotionDTO promotion = _mapper.Map<Promotion, PromotionDTO>(activePromotion);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Current Active Bank Promotion : {promotion.Message}");
            return new ResultDTO<PromotionDTO>(true,200,"",promotion);
        }
        #endregion
        
    }
}
