using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess.Repositories
{
    class CollateralRepository : RepositoryBase<Collateral> , ICollateralRepository
    {
        public CollateralRepository(DatabaseContext context):base(context){}

        
    }
}
