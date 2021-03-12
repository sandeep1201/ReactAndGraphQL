using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Model;

namespace Dcf.Wwp.DataAccess.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties

        private readonly IDatabaseFactory _databaseFactory;
        private          IDbContext       _dataContext;
        private          IDbContext       DataContext => _dataContext ?? (_dataContext = _databaseFactory.GetContext());

        #endregion

        #region Methods

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public int Commit()
        {   
            return(DataContext.Commit());
        }

        public CommitStatus CommitWithStatus()
        {
            return DataContext.CommitWithStatus();
        }

        public async Task<int> CommitAsync()
        {
            return (await DataContext.CommitAsync());
        }

        public void RollBack()
        {
            DataContext.Rollback();
        }

        public void SetEntityModified<T>(int id) where T : class
        {
            DataContext.SetEntityModified<T>(id);
        }

        #endregion
    }
}
