using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Contracts
{
    public interface IRepositoryManager
    {
        ICityRepository City { get; }
        ICollateralRepository Collateral { get; }
        ICountryRepository Country { get; }
        ILoanRepository Loan { get; }
        ILoanCollateralRepository LoanCollateral { get; }
        ILoanStateRepository LoanState { get; }
        IPromotionRepository Promotion { get; }
        IStateRepository State { get; }
        void Save();
    }
}
