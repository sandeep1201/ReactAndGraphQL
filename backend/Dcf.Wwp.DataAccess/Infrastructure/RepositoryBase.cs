using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Interfaces;

namespace Dcf.Wwp.DataAccess.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        #region Properties

        private readonly WwpContext _dataContext;
        private readonly DbSet<T>   _dbset;

        #endregion

        #region Methods

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            _dataContext = (WwpContext)databaseFactory.GetContext();
            _dbset = _dataContext.Set<T>();
        }

        //TODO: insert IDbContext and use this one instead the above
        //protected RepositoryBase(IDbContext dataContext)
        //{
        //    _dataContext = (SchoolDbContext)dataContext;
        //    _dbset = _dataContext.Set<T>();
        //}
        
        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var entities = _dbset.Where(where).ToList();

            entities.ForEach(i => _dbset.Remove(i));
        }

        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }

        public virtual T GetById(string id)
        {
            return _dbset.Find(id);
        }

        public T Get(Expression<Func<T, bool>> clause)
        {
            return _dbset.FirstOrDefault(clause);
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> clause)
        {
            var r = new List<T>();

            try
            {
                r = _dbset.Where(clause).ToList();
            }
            catch (NullReferenceException)
            {
                // insert log4net.Debug here
            }

            return (r);
        }

        public virtual IEnumerable<T> GetManyWithDetails(Expression<Func<T, bool>> clause, string navProps)
        {
            try
            {
                return (_dbset.Include(navProps).Where(clause).ToList());
            }
            catch (NullReferenceException)
            {
                return new List<T>();
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            return (_dbset.ToList());
        }

        public virtual Task<T> GetByIdAsync(long id)
        {
            return _dbset.FindAsync(id);
        }

        public virtual Task<T> GetByIdAsync(string id)
        {
            return _dbset.FindAsync(id);
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> clause)
        {
            return _dbset.FirstOrDefaultAsync(clause);
        }
        
        public virtual async Task<List<T>> GetManyAsync(Expression<Func<T, bool>> clause)
        {
            return (await _dbset.Where(clause).ToListAsync());
        }

        public virtual async Task<List<T>> GetManyWithDetailsAsync(Expression<Func<T, bool>> clause, string details)
        {
            return await _dbset.Include(details).Where(clause).ToListAsync();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return (await _dbset.ToListAsync());
        }

        public virtual IQueryable<T> GetAsQueryable()
        {
            return _dbset.AsQueryable();
        }

        public void ExecuteRawSql(string sql)
        {
            _dataContext.Database.ExecuteSqlCommand(sql);
        }

        public virtual IEnumerable<T> SqlQuery(string statement)
        {
            return _dbset.SqlQuery(statement);
        }

        public DataTable ExecStoredProcUsingAdo(string storedProcName, Dictionary<string, object> parms = null)
        {
            var dtResults = _dataContext.ExecStoredProcUsingAdo(storedProcName, parms);
            
            return (dtResults);
        }

        public virtual IList<TC> ExecStoredProc<TC>(string storedProcName)
        {
            var results = (List<TC>)_dataContext.ExecStoredProc<TC>(storedProcName);

            return (results);
        }

        public virtual IList<TC> ExecStoredProc<TC>(string storedProcName, Dictionary<string, object> parms)
        {
            var results = _dataContext.ExecStoredProc<TC>(storedProcName, parms);

            return (results);
        }

        #endregion
    }
}
