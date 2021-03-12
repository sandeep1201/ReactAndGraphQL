using Dcf.Wwp.DataAccess.Interfaces;

namespace Dcf.Wwp.DataAccess.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        #region Properties

        private IDbContext _dataContext;

        #endregion

        #region Methods

        public DatabaseFactory(IDbContext dbContext)
        {
            _dataContext = dbContext;
        }

        public IDbContext GetContext()
        {
            return (_dataContext);
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
        }

        #endregion
    }
}
