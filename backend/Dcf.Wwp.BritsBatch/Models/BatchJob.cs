using System.Data;
using log4net;
using Dcf.Wwp.BritsBatch.Interfaces;

namespace Dcf.Wwp.BritsBatch.Models
{
    public abstract class BatchJob : IBatchJob
    {
        #region Properties

        // protected readonly ILog      _log;
        // protected readonly IDbConfig _dbConfig;
        //
        // protected BatchJob(IDbConfig dbConfig, ILog log)
        // {
        //     _dbConfig = dbConfig;
        //     _log      = log;
        // }

        // public virtual string Name    { get; }
        // public virtual string Desc    { get; }
        // public virtual string Sproc   { get; }
        // public virtual int    NumRows { get; protected set; }

        #endregion

        #region Methods

        //public virtual DataTable Run() => null;

        #endregion
    }
}
