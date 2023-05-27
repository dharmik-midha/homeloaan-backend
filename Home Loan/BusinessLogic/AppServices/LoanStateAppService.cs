using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.DBContext;
using Logging.NLog;
using Microsoft.AspNetCore.Http;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Constants.Values;
using Constants.Enums;
using DataAccess.Contracts;

namespace BusinessLogic.AppServices
{
    public class LoanStateAppService : ILoanStateAppService
    {
        #region Private Variables
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        private const string loggerPrefix = nameof(LoanStateAppService);
        #endregion

        #region ctor
        public LoanStateAppService(
            ILogger logger,
            IMapper mapper,
            IRepositoryManager repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }
        #endregion

        #region Add Loan State
        /// <summary>
        /// Add Loan State
        /// </summary>
        /// <param name="loanStateDTO"></param>
        /// <returns></returns>
        public ResultDTO AddLoanState(LoanStateDTO loanStateDTO)
        {
            const string localLoggerPrefix = nameof(AddLoanState);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started");
            LoanState loanState = _mapper.Map<LoanStateDTO, LoanState>(loanStateDTO);
            loanState.Id = Guid.NewGuid().ToString();
            loanState.Date = DateTime.Now;
            _repository.LoanState.Create(loanState);
            _repository.Save();
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan State Added (ID : {loanState.Id})");
            return new ResultDTO(true, StatusCodes.Status200OK, "");
        }
        #endregion

        #region Get Current Loan State
        /// <summary>
        /// Get Current Loan State
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDTO<LoanStateDTO> GetCurrentLoanState(string id)
        {
            const string localLoggerPrefix = nameof(GetCurrentLoanState);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|loan state ID : {id}|started");
            var loanApplicationList = GetLoanStateHistory(id);
            if (!loanApplicationList.Success)
            {
                return new ResultDTO<LoanStateDTO>(false, StatusCodes.Status404NotFound, $"Loan application not found.", null);
            }
            var loanApplication = loanApplicationList.Data.FirstOrDefault();

            if (loanApplication == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan application with ID {id} not found.");
                return new ResultDTO<LoanStateDTO>(false, StatusCodes.Status404NotFound, $"Loan application not found.", null);
            }

            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Current state : {loanApplication.State}");
            return new ResultDTO<LoanStateDTO>(true, StatusCodes.Status200OK, "", loanApplication);
        }
        #endregion

        #region Get Loan State History
        /// <summary>
        /// Get Loan State History
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDTO<IList<LoanStateDTO>> GetLoanStateHistory(string id)
        {
            const string localLoggerPrefix = nameof(GetLoanStateHistory);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|loan state ID : {id}|started");
            var loanState = _repository.LoanState.FindByCondition(x => x.LoanID == id, false);
            if (loanState.Count() == 0)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|No History Found with id : {id}");
                return new ResultDTO<IList<LoanStateDTO>>(false, StatusCodes.Status404NotFound, "Loan Id Not Found", null);
            }
            //implicit converted 
            IList<LoanState> stateHistoryList = loanState
                .OrderByDescending(x => x.Date)
                .ToList();
            IList<LoanStateDTO> LoanStateDto = _mapper.Map<IList<LoanState>, IList<LoanStateDTO>>(stateHistoryList);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan State History Found with id : {id}");
            return new ResultDTO<IList<LoanStateDTO>>(true, StatusCodes.Status200OK, "", LoanStateDto);
        }
        #endregion

        #region Change Loan State (Advisor)
        /// <summary>
        /// Change Loan state by advisor
        /// </summary>
        /// <param name="loanStateDTO"></param>
        /// <returns></returns>
        public ResultDTO ChangeLoanState(LoanStateDTO loanStateDTO)
        {
            const string localLoggerPrefix = nameof(ChangeLoanState);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|loan state ID : {loanStateDTO.LoanID}|started");

            var currentLoanState = GetCurrentLoanState(loanStateDTO.LoanID);

            if (!currentLoanState.Success)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Current state : {currentLoanState.Data.State}");
                return currentLoanState;
            }

            var loanStateCheck = LoanNewStateCheck(currentLoanState.Data.State, loanStateDTO.State);

            if (!loanStateCheck)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Invalid Loan State Conversion");
                return new ResultDTO(false, StatusCodes.Status400BadRequest, "Invalid Loan State Conversion");
            }

            AddLoanState(loanStateDTO);

            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan (ID : {loanStateDTO.LoanID}) State Changed  to {loanStateDTO.State} by Advisor {loanStateDTO.UserID}");
            return new ResultDTO(true, StatusCodes.Status200OK, "");
        }
        #endregion

        private bool LoanNewStateCheck(LoanStates oldLoanState, LoanStates newLoanState)
        {
            IList<LoanStates> possibleNewState = LoanStateChangeValue.value[oldLoanState];

            // Check if the Conversion from old state to new state is possible
            if (!possibleNewState.Contains(newLoanState))
            {
                return false;
            }

            return true;
        }
    }
}
