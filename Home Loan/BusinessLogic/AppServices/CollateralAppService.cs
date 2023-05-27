using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using Logging.NLog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.AppServices
{
    public class CollateralAppService : ICollateralAppService
    {
        #region Private Variables
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private const string loggerPrefix = nameof(CollateralAppService);
        #endregion

        #region ctor
        public CollateralAppService(
            IRepositoryManager repository, 
            IMapper mapper, 
            ILogger logger
            )
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region AddCollateral
        /// <summary>
        /// Add Collateral
        /// </summary>
        /// <param name="collateralDto"></param>
        /// <returns></returns>
        public ResultDTO AddCollateral(CollateralDTO collateralDto)
        {
            const string localLoggerPrefix = nameof(AddCollateral);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Collateral ID : {collateralDto.Id}|started.");
            
            Collateral collateral = _mapper.Map<CollateralDTO, Collateral>(collateralDto);
            collateral.Id = Guid.NewGuid().ToString();
            _repository.Collateral.Create(collateral);
            _repository.Save();
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Colleteral Added with id : {collateral.Id}|Completed.");


            return new ResultDTO(true,201,"Collateral added");
        }
        #endregion

        #region Update Collateral
        /// <summary>
        /// Update Collateral
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collateralDto"></param>
        /// <returns></returns>
        public ResultDTO UpdateCollateral(string id, CollateralDTO collateralDto)
        {
            const string localLoggerPrefix = nameof(UpdateCollateral);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Collateral ID : {collateralDto.Id}|started.");
            // Find the existing collateral
            var existingCollateral = _repository.Collateral.GetByKey(id);

            // If the collateral doesn't exist, return a 404 error
            if (existingCollateral == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Colateral Not Found with id : {id}");
                return new ResultDTO(false,404, "Collateral Doesn't Exist");
            }

            // Update Collateral
            existingCollateral.CollateralValue = collateralDto.CollateralValue;
            existingCollateral.OwnShare = collateralDto.OwnShare;
            _repository.Save();
            _logger.LogInfo($"{loggerPrefix} | {localLoggerPrefix} :Colateral Updated with id : {id}");

            return new ResultDTO(true,200,"Updated");
        }
        #endregion

        #region GetAllCollateral
        /// <summary>
        /// Get All Collateral
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ResultDTO<IList<Collateral>> GetAllCollateral(string email)
        {
            //Get Collaterals
            const string localLoggerPrefix = nameof(GetAllCollateral);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started.");

            var collaterals = _repository.Collateral
                .FindByCondition(collateral => collateral.OwnerEmail == email,false)
                .ToList();
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|{collaterals.Count} Colaterals Found for user : {email}|completed.");
            return new ResultDTO<IList<Collateral>>(true,200,"Ok",collaterals);
        }
        #endregion

        #region DeleteCollateral
        /// <summary>
        /// Delete Collateral
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDTO DeleteCollateral(string id)
        {
            const string localLoggerPrefix = nameof(DeleteCollateral);
            var collateral = _repository.Collateral.GetByKey(id);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|collateral ID : {id}|started.");

            // If the collateral doesn't exist, return a 404 error
            if (collateral == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Colateral Not Found with id : {id}");
                return new ResultDTO(false,404, "Collateral Doesn't Exist");
            }
            IList<LoanCollateral> loanCollateral = _repository.LoanCollateral
                .FindByCondition(x => x.CollateralId == id,false)
                .ToList();
            if (loanCollateral.Count != 0)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Colateral is used in a loan. Deletion failed!");
                return new ResultDTO(false,403, "Collateral is used in a loan");
            }

            _repository.Collateral.Delete(collateral);
            _repository.Save();
            _logger.LogInfo($"{loggerPrefix} | {localLoggerPrefix} : Colateral Removed with id : {id}");
            return new ResultDTO(true,200,"Deleted successfuly");
        }
        #endregion
    }
}
