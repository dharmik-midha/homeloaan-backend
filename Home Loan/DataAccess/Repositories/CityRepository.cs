using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess.Repositories
{
    class CityRepository : RepositoryBase<City> , ICityRepository
    {
        public CityRepository(DatabaseContext context):base(context){}

        
    }
}
