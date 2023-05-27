using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess.Repositories
{
    class LoanRepository : RepositoryBase<Loan> ,ILoanRepository
    {
        public LoanRepository(DatabaseContext context):base(context){}
    }
}
