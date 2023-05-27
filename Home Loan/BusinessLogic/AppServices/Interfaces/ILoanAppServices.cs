using BusinessLogic.DTO;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.AppServices
{
    public interface ILoanAppServices
    {
        ResultDTO<IList<LoanViewDTO>> GetAllLoans();
        ResultDTO CreateNewLoan(LoanDTO item);
        ResultDTO ApplyLoan(string email, string loanId);
        ResultDTO AddCollateral(string email, string loanId, string collateralId);
        ResultDTO<IList<CollateralDTO>> GetCollateralInLoan(string loanId);
        ResultDTO<IList<LoanDTO>> GetUserLoans(string email);
        ResultDTO<LoanDTO> GetUserLoan(string email, string LoanId);
        ResultDTO RemoveCollateralFromLoan(string userEmail, string loanId, string collateralId);
        ResultDTO EditLoanValues(LoanDTO editvalues);
        ResultDTO<IList<LoanStateDTO>> GetLoanStateHistory(string email, string id);
    }
}
