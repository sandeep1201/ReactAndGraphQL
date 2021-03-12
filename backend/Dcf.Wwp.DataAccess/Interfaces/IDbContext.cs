using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface.Model;

namespace Dcf.Wwp.DataAccess.Interfaces
{
    public interface IDbContext : IDisposable, IObjectContextAdapter
    {
        int          Commit();
        CommitStatus CommitWithStatus();
        Task<int>    CommitAsync();
        void         Rollback();
        void         SetEntityModified<T>(int id) where T : class;
        DataTable    ExecStoredProcUsingAdo(string   storedProcName, Dictionary<string, object> parms = null);
        IList<T>     ExecStoredProc<T>(string        storedProcName);
        IList<T>     ExecStoredProc<T>(string        storedProcName, Dictionary<string, object> parms);
        int          GetStoredProcReturnValue(string storedProcName, Dictionary<string, object> parms);
        IList<T>     ExecFunction<T>(string          functionName,   Dictionary<string, object> parms, bool isTableValuedFunction);
    }
}
