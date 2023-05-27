using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess.Repositories
{
    class StateRepository : RepositoryBase<State> ,IStateRepository
    {
        public StateRepository(DatabaseContext context):base(context){}

        
    }
}
