using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess.Repositories
{
    class CountryRepository : RepositoryBase<Country> , ICountryRepository
    {
        public CountryRepository(DatabaseContext context):base(context){}

        
    }
}
