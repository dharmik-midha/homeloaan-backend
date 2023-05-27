using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess.Repositories
{
    class LoanStateRepository : RepositoryBase<LoanState> , ILoanStateRepository
    {
        public LoanStateRepository(DatabaseContext context):base(context){}

        
    }
}
