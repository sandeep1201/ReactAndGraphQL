using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dcf.Wwp.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void           Add(T                                        entity);
        void           Update(T                                     entity);
        void           AddOrUpdate(T                                entity);
        void           Delete(T                                     entity);
        void           Delete(Expression<Func<T, bool>>             clause);
        void           DeleteRange(IEnumerable<T>                   entities);
        T              GetById(long                                 id);
        T              GetById(string                               id);
        T              Get(Expression<Func<T, bool>>                clause);
        IEnumerable<T> GetMany(Expression<Func<T, bool>>            clause);
        IEnumerable<T> GetManyWithDetails(Expression<Func<T, bool>> clause, string navProps);
        IEnumerable<T> GetAll();
        Task<T>        GetByIdAsync(long                                 id);
        Task<T>        GetByIdAsync(string                               id);
        Task<T>        GetAsync(Expression<Func<T, bool>>                clause);
        Task<List<T>>  GetManyAsync(Expression<Func<T, bool>>            clause);
        Task<List<T>>  GetManyAsync2(Expression<Func<T, bool>>           clause);
        Task<List<T>>  GetManyWithDetailsAsync(Expression<Func<T, bool>> clause, string details);
        Task<List<T>>  GetAllAsync();
        T              New();
        IQueryable<T>  GetAsQueryable(bool             withTracking = true);
        void           ExecuteRawSql(string            statement);
        IEnumerable<T> SqlQuery(string                 statement);
        IList<TC>      ExecFunction<TC>(string         functionName,   Dictionary<string, object> parms, string schemaName = "wwp", bool isTableValuedFunction = false);
        IList<TC>      ExecStoredProc<TC>(string       storedProcName, Dictionary<string, object> parms, string schemaName = "wwp");
        int            GetStoredProcReturnValue(string storedProcName, Dictionary<string, object> parms, string schemaName = "wwp");
        string         GetServerName();
        Database       GetDataBase();
    }
}
