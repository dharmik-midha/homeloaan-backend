using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Repositories;


namespace DataAccess.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private DatabaseContext _context;
        private ICityRepository _cityRepository;
        private ICollateralRepository _collateralRepository;
        private ICountryRepository _countryRepository;
        private ILoanRepository _loanRepository;
        private ILoanCollateralRepository _loanCollateralRepository;
        private ILoanStateRepository _loanStateRepository;
        private IPromotionRepository _promotionRepository;
        private IStateRepository _stateRepository;


        public RepositoryManager(DatabaseContext context)
        {
            _context = context;
        }

        

        public ICityRepository City
        {
            get
            {
                if (_cityRepository == null)
                    _cityRepository = new CityRepository(_context);
                return _cityRepository;
            }
        }

        public ICollateralRepository Collateral
        {
            get
            {
                if (_collateralRepository == null)
                    _collateralRepository = new CollateralRepository(_context);
                return _collateralRepository;
            }
        }

        public ICountryRepository Country
        {
            get
            {
                if (_countryRepository == null)
                    _countryRepository = new CountryRepository(_context);
                return _countryRepository;
            }
        }
        public ILoanRepository Loan
        {
            get
            {
                if (_loanRepository == null)
                    _loanRepository = new LoanRepository(_context);
                return _loanRepository;
            }
        }
        public ILoanCollateralRepository LoanCollateral
        {
            get
            {
                if (_loanCollateralRepository == null)
                    _loanCollateralRepository = new LoanCollateralRepository(_context);
                return _loanCollateralRepository;
            }
        }

        public ILoanStateRepository LoanState
        {
            get
            {
                if (_loanStateRepository == null)
                    _loanStateRepository = new LoanStateRepository(_context);
                return _loanStateRepository;
            }
        }

        public IPromotionRepository Promotion
        {
            get
            {
                if (_promotionRepository == null)
                    _promotionRepository = new PromotionRepository(_context);
                return _promotionRepository;
            }
        }

        public IStateRepository State
        {
            get
            {
                if (_stateRepository == null)
                    _stateRepository = new StateRepository(_context);
                return _stateRepository;
            }
        }

        public void Save() => _context.SaveChanges();
    }
}
