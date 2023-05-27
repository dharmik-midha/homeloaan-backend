using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.DBContext;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Logging.NLog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Constants.Enums;
using Constants.Values;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DataAccess.Contracts;

namespace BusinessLogic.AppServices
{
    public class LoanAppServices : ILoanAppServices
    {
        #region Private Variables
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        private readonly ILoanStateAppService _loanStateAppService;
        private readonly UserManager<IdentityUser> userManager;
        private const string loggerPrefix = nameof(LoanAppServices);
        #endregion

        #region ctor
        public LoanAppServices(
            ILogger logger,
            IMapper mapper,
            IRepositoryManager repository,
            UserManager<IdentityUser> userManager,
            ILoanStateAppService loanStateAppService)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _loanStateAppService = loanStateAppService;
            this.userManager = userManager;
        }
        #endregion

        #region CreateNewLoan
        /// <summary>
        /// Create New Loan
        /// </summary>
        /// <param name="loanItem"></param>
        /// <returns></returns>
        public ResultDTO CreateNewLoan(LoanDTO loanItem)
        {
            const string localLoggerPrefix = nameof(CreateNewLoan);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started.");
            Loan loan = _mapper.Map<LoanDTO, Loan>(loanItem);
            loan.LoanId = Guid.NewGuid().ToString();
            _repository.Loan.Create(loan);
            _repository.Save();
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan Added (ID : {loan.LoanId})");

            //Get user id
            var UserEmail = loanItem.UserEmail;
            IdentityUser user = userManager.FindByEmailAsync(UserEmail).Result;

            //send loan state
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Setting Loan state to created.");
            LoanStateDTO loanStateDto = new LoanStateDTO();
            loanStateDto.LoanID = loan.LoanId;
            loanStateDto.State = LoanStates.Created;
            loanStateDto.UserID = user.Id;
            _loanStateAppService.AddLoanState(loanStateDto);

            return new ResultDTO(true, StatusCodes.Status200OK, "");
        }
        #endregion

        #region Apply Loan
        /// <summary>
        /// Get All Loans
        /// </summary>
        /// <returns></returns>
        public ResultDTO ApplyLoan(string email, string loanId)
        {
            const string localLoggerPrefix = nameof(ApplyLoan);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|loan ID : {loanId}|started.");

            //Loan Loans = _context.Loan.Find(loanId);
            Loan loan = _repository.Loan.GetByKey(loanId);


            if (loan == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|loan ID : {loanId}|Loan not Found.");
                return new ResultDTO(false, StatusCodes.Status404NotFound, "Loan not found");
            }
            if (loan.userEmail != email)
            {
                return new ResultDTO(false, StatusCodes.Status403Forbidden, "Forbidden!");
            }

            var loanState = _loanStateAppService.GetCurrentLoanState(loanId);

            // Checking if loan state exists and is in "Created" state.
            if (!loanState.Success || loanState.Data.State != LoanStates.Created)
            {
                return new ResultDTO(false, StatusCodes.Status403Forbidden, "Forbidden!");
            }

            LoanStateDTO loanStateDto = new LoanStateDTO()
            {
                LoanID = loanId,
                State = LoanStates.InProgress,
                UserID = email
            };

            _loanStateAppService.AddLoanState(loanStateDto);

            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|loan ID : {loanId}|Loan Applied Successfully|completed.");
            return new ResultDTO(true, StatusCodes.Status200OK, "Loan Applied Successfully");
        }
        #endregion

        #region GetAllLoans
        /// <summary>
        /// Get All Loans
        /// </summary>
        /// <returns></returns>
        public ResultDTO<IList<LoanViewDTO>> GetAllLoans()
        {
            const string localLoggerPrefix = nameof(GetAllLoans);
            List<LoanViewDTO> loanViews = new List<LoanViewDTO>();
            List<Loan> Loans = _repository.Loan.FindAll(false).ToList();
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Total {Loans.Count} Loans Found|started.");
            foreach (var loan in Loans)
            {
                LoanViewDTO loanCollateral = new LoanViewDTO();

                loanCollateral.loan = _mapper.Map<Loan, LoanDTO>(loan);
                loanCollateral.collaterals = GetCollateralInLoan(loan.LoanId).Data;

                loanCollateral.TotalColleteralValue = loanCollateral.collaterals
                    .Sum(x => (x.CollateralValue * x.OwnShare * ColleteralValues.value[x.CollateralType]) / 100000);

                loanCollateral.status = LoanStatusValue.getLoanStatus(loanCollateral.TotalColleteralValue * 100 / loan.LoanAmount);
                loanCollateral.state = _loanStateAppService.GetCurrentLoanState(loanCollateral.loan.LoanId).Data.State;
                loanViews.Add(loanCollateral);
            }
            return new ResultDTO<IList<LoanViewDTO>>(true, StatusCodes.Status200OK, "", loanViews);
        }
        #endregion

        #region Add Collateral to loan
        /// <summary>
        /// Add Collateral To Loan
        /// </summary>
        /// <param name="email"></param>
        /// <param name="loanCollateralDto"></param>
        /// <returns></returns>
        public ResultDTO AddCollateral(string email, string loanId, string collateralId)
        {
            const string localLoggerPrefix = nameof(AddCollateral);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|loan ID : {loanId}|started.");
            //Loan loan = _context.Loan.Find(loanId);
            Loan loan = _repository.Loan.GetByKey(loanId);
            //Collateral collateral = _context.Collateral.Find(collateralId);
            Collateral collateral = _repository.Collateral.GetByKey(collateralId);
            if (loan == null || collateral == null)
            {
                return new ResultDTO(false, 400, "Either Loan or Collateral not found.");
            }
            if (loan.userEmail != email || collateral.OwnerEmail != email)
            {
                // User does not have either loan or collateral access 
                return new ResultDTO(false, 403, "Forbidden!");
            }

            LoanCollateral loanCollateral = new LoanCollateral()
            {
                Id = Guid.NewGuid().ToString(),
                CollateralId = collateralId,
                LoanId = loanId
            };
            _repository.LoanCollateral.Create(loanCollateral);
            _repository.Save();
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Added Collateral ID : {collateral.Id} to Loan ID : {loan.LoanId}|completed ");

            return new ResultDTO(true, 200, $"Collateral added to Loan ({loan.LoanId})");
        }
        #endregion

        #region Get Collateral In Loan
        /// <summary>
        /// Get Collateral In Loan
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        public ResultDTO<IList<CollateralDTO>> GetCollateralInLoan(string loanId)
        {
            //TODO : check if the requester is either advisor or user who owns the loan
            const string localLoggerPrefix = nameof(GetCollateralInLoan);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|loan ID : {loanId}|started.");
            if (_repository.Loan.GetByKey(loanId) == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|No Loan Found with id : {loanId}");
                return new ResultDTO<IList<CollateralDTO>>(false, 404, "Loan not Found", null);
            }

            List<string> loanCollateralIdList = _repository.LoanCollateral
                .FindByCondition(x => x.LoanId == loanId, false)
                .Select(lc => lc.CollateralId)
                .ToList();

            IList<Collateral> collateralList = _repository.Collateral
                .FindByCondition(x => loanCollateralIdList.Contains(x.Id), false)
                .ToList();

            IList<CollateralDTO> collateralsDTOList = _mapper.Map<IList<Collateral>, IList<CollateralDTO>>(collateralList);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|{collateralsDTOList.Count} Colaterals found for loan with id : {loanId}");

            return new ResultDTO<IList<CollateralDTO>>(true, 200, "Ok", collateralsDTOList);
        }
        #endregion

        #region Remove Collateral From Loan
        /// <summary>
        /// Remove Collateral from loan
        /// </summary>
        /// <param name="loanCollateralId"></param>
        /// <returns></returns>
        public ResultDTO RemoveCollateralFromLoan(string email, string loanId, string collateralId)
        {
            //also can only be done if the loan is not active
            const string localLoggerPrefix = nameof(RemoveCollateralFromLoan);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|loan ID : {loanId}|collateral ID : {collateralId}|started.");

            Loan loan = _repository.Loan.GetByKey(loanId);
            Collateral collateral = _repository.Collateral.GetByKey(collateralId);
            if (loan == null || collateral == null)
            {
                return new ResultDTO(false, 400, "Either Loan or Collateral not found.");
            }

            if (loan.userEmail != email || collateral.OwnerEmail != email)
            {
                // User does not have either loan or collateral access 
                return new ResultDTO(false, 403, "Forbidden!");
            }

            LoanCollateral loanCollateral = _repository.LoanCollateral
                .FindByCondition((x) => x.CollateralId == collateralId && x.LoanId == loanId, false)
                .FirstOrDefault();

            if (loanCollateral == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan-Colateral Not Found with loanid : {loanId} & collateralid : {collateralId} ");
                return new ResultDTO(false, 404, "Collateral does not exist in the given loan id");
            }

            _repository.LoanCollateral.Delete(loanCollateral);
            _repository.Save();
            _logger.LogInfo($"{loggerPrefix} | {localLoggerPrefix} : Collateral removed from Loan");
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Collateral removed from Loan");

            return new ResultDTO(true, 200, "collateral removed from loan");
        }
        #endregion

        #region Get All User Loans
        /// <summary>
        /// Get All User loans
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ResultDTO<IList<LoanDTO>> GetUserLoans(string email)
        {
            const string localLoggerPrefix = nameof(GetUserLoans);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|started.");
            List<Loan> loans = _repository.Loan.FindByCondition((loan) => loan.userEmail == email, false).ToList();
            IList<LoanDTO> data = new List<LoanDTO>();
            foreach (var loan in loans)
            {
                LoanDTO loanDto = _mapper.Map<Loan, LoanDTO>(loan);
                loanDto.state = _loanStateAppService.GetCurrentLoanState(loan.LoanId).Data.State;
                data.Add(loanDto);
            }
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|{loans.Count}: Loans Found for user : {email}");

            return new ResultDTO<IList<LoanDTO>>(true, 200, "Ok", data);
        }
        #endregion

        #region Get User Loan
        /// <summary>
        /// Get User Loan
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDTO<LoanDTO> GetUserLoan(string email, string id)
        {
            const string localLoggerPrefix = nameof(GetUserLoan);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|started.");
            Loan loan = _repository.Loan.GetByKey(id);
            if (loan == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|Loan Not Found.");
                return new ResultDTO<LoanDTO>(false, 404, "Loan Not Found", null);
            }
            if (loan.userEmail != email)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|Unauthorized.");
                return new ResultDTO<LoanDTO>(false, 403, "Unauthorized", null);
            }

            LoanDTO data = _mapper.Map<Loan, LoanDTO>(loan);
            data.state = _loanStateAppService.GetCurrentLoanState(loan.LoanId).Data.State;
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan Found for user : {email}");
            return new ResultDTO<LoanDTO>(true, 200, "Ok", data);
        }
        #endregion

        #region Get Loan State History
        /// <summary>
        /// Get Loan State History
        /// </summary>
        /// <returns></returns>
        public ResultDTO<IList<LoanStateDTO>> GetLoanStateHistory(string email, string id)
        {
            const string localLoggerPrefix = nameof(GetLoanStateHistory);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|started.");
            Loan loan = _repository.Loan.GetByKey(id);
            if (loan == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|Loan Not Found.");
                return new ResultDTO<IList<LoanStateDTO>>(false, 404, "Loan Not Found", null);
            }
            if (loan.userEmail != email)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|email : {email}|Unauthorized.");
                return new ResultDTO<IList<LoanStateDTO>>(false, 403, "Unauthorized", null);
            }

            IList<LoanStateDTO> data = _loanStateAppService.GetLoanStateHistory(loan.LoanId).Data;
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan Found for user : {email}");
            return new ResultDTO<IList<LoanStateDTO>>(true, 200, "Ok", data);
        }
        #endregion

        #region EditLoan
        /// <summary>
        /// Edit Loan
        /// </summary>
        /// <param name="editvalues"></param>
        /// <returns></returns>
        public ResultDTO EditLoanValues(LoanDTO editvalues)
        {
            const string localLoggerPrefix = nameof(EditLoanValues);
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|started.");
            var existingLoan = _repository.Loan.GetByKey(editvalues.LoanId);
            if (existingLoan == null)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan Not Found : {editvalues.UserEmail}");
                return new ResultDTO(false, 404, "Loan Doesn't Exist");
            }

            if (existingLoan.userEmail != editvalues.UserEmail)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|user : {editvalues.UserEmail} does not have rights.");
                return new ResultDTO(false, 403, "Unauthorised loan modification.");
            }

            var loanState = _loanStateAppService.GetCurrentLoanState(existingLoan.LoanId);
            if (loanState.Data.State != LoanStates.Created)
            {
                _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan is submitted");
                return new ResultDTO(false, 400, "Loan is already submitted.");
            }
            existingLoan.LoanAmount = editvalues.LoanAmount;
            existingLoan.LoanDuration = editvalues.LoanDuration;
            existingLoan.MonthlyFamilyIncome = editvalues.MonthlyFamilyIncome;
            existingLoan.OtherIncome = editvalues.OtherIncome;
            existingLoan.PropertyAddress = editvalues.PropertyAddress;
            existingLoan.PropertyCost = editvalues.PropertyCost;
            existingLoan.PropertyRegistrationCost = editvalues.PropertyRegistrationCost;
            existingLoan.PropertySize = editvalues.PropertySize;

            _repository.Loan.Update(existingLoan);
            _repository.Save();
            _logger.LogInfo($"{loggerPrefix}|{localLoggerPrefix}|Loan values Updated : {editvalues.UserEmail}");
            return new ResultDTO(true, 200, "Updated");
        }
        #endregion
    }
}
