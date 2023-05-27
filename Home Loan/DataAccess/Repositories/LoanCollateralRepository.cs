using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess.Repositories
{
    class LoanCollateralRepository : RepositoryBase<LoanCollateral> , ILoanCollateralRepository
    {
        public LoanCollateralRepository(DatabaseContext context):base(context){}

        
    }
}
