using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess.Repositories
{
    class PromotionRepository : RepositoryBase<Promotion> , IPromotionRepository
    {
        public PromotionRepository(DatabaseContext context):base(context){}

        
    }
}
