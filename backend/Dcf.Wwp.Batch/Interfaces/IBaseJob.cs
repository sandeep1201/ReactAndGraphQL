using System;
using System.Data;
using System.Data.SqlClient;
using log4net;

namespace Dcf.Wwp.Batch.Interfaces
{
    public interface IBaseJob
    {
        #region Properties

        IDbConfig DbConfig    { get; }
        ILog      Log         { get; }
        string    ProgramCode { get; }
        DataTable DataTable   { get; }

        #endregion

        #region Methods

        void      RunSproc(string name, string sproc = null, SqlParameter[] parameters = null);
        DataTable ExecSql(string  sql);

        #endregion
    }
}
