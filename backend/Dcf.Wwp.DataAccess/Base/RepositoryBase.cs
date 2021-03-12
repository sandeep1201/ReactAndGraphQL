using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Interfaces;

namespace Dcf.Wwp.DataAccess.Base
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        #region Properties

        private readonly BaseDbContext _dataContext;
        private readonly DbSet<T>      _dbSet;

        #endregion

        #region Methods

        protected RepositoryBase(IDbContext dataContext)
        {
            _dataContext = (BaseDbContext) dataContext;
            _dbSet       = _dataContext.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void AddOrUpdate(T entity)
        {
            _dataContext.Set<T>().AddOrUpdate(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            _dbSet.Where(where).AsEnumerable().ForEach(obj => _dbSet.Remove(obj));
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual T GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> clause)
        {
            return _dbSet.FirstOrDefault(clause);
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> clause)
        {
            try
            {
                return _dbSet.Where(clause).ToList();
            }
            catch (NullReferenceException)
            {
                return new List<T>();
            }
        }

        public virtual IEnumerable<T> GetManyWithDetails(Expression<Func<T, bool>> clause, string navProps)
        {
            try
            {
                return _dbSet.Include(navProps).Where(clause).ToList();
            }
            catch (NullReferenceException)
            {
                return new List<T>();
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual Task<T> GetByIdAsync(long id)
        {
            return _dbSet.FindAsync(id);
        }

        public virtual Task<T> GetByIdAsync(string id)
        {
            return _dbSet.FindAsync(id);
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> clause)
        {
            return _dbSet.FirstOrDefaultAsync(clause);
        }

        public virtual Task<List<T>> GetManyAsync(Expression<Func<T, bool>> clause)
        {
            return _dbSet.Where(clause).ToListAsync();
        }

        public virtual async Task<List<T>> GetManyAsync2(Expression<Func<T, bool>> clause)
        {
            return await _dbSet.Where(clause).ToListAsync();
        }

        public virtual Task<List<T>> GetManyWithDetailsAsync(Expression<Func<T, bool>> clause, string details)
        {
            return _dbSet.Include(details).Where(clause).ToListAsync();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        public virtual IQueryable<T> GetAsQueryable(bool withTracking = true)
        {
            return withTracking ? _dbSet.AsQueryable() : _dbSet.AsQueryable().AsNoTracking();
        }

        public virtual T New()
        {
            return _dbSet.Create<T>();
        }

        public void ExecuteRawSql(string sql)
        {
            _dataContext.Database.ExecuteSqlCommand(sql);
        }

        public virtual IEnumerable<T> SqlQuery(string statement)
        {
            return _dbSet.SqlQuery(statement);
        }

        public virtual IList<TC> ExecFunction<TC>(string functionName, Dictionary<string, object> parms, string schemaName = "wwp", bool isTableValuedFunction = false)
        {
            return (List<TC>) _dataContext.ExecFunction<TC>(string.Concat(schemaName, '.', functionName), parms, isTableValuedFunction);
        }

        public virtual IList<TC> ExecStoredProc<TC>(string storedProcName, Dictionary<string, object> parms, string schemaName = "wwp")
        {
            return _dataContext.ExecStoredProc<TC>(string.Concat(schemaName, '.', storedProcName), parms);
        }

        public virtual int GetStoredProcReturnValue(string storedProcName, Dictionary<string, object> parms, string schemaName = "wwp")
        {
            return _dataContext.GetStoredProcReturnValue(string.Concat(schemaName, '.', storedProcName), parms);
        }

        public string GetServerName()
        {
            return _dataContext.Database.Connection.DataSource;
        }

        public Database GetDataBase()
        {
            return _dataContext.Database;
        }

        #endregion
    }
}
