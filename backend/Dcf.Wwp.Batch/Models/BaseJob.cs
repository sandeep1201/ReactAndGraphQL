using System.Data;
using System.Data.SqlClient;
using log4net;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Models
{
    public class BaseJob : IBaseJob
    {
        #region Properties

        public           IDbConfig DbConfig    { get; }
        public           ILog      Log         { get; }
        public           string    ProgramCode { get; }
        public           DataTable DataTable   { get; private set; }
        private          string    Sproc       { get; set; }
        private readonly bool      _isSimulation;

        #endregion

        #region Methods

        public BaseJob(IDbConfig dbConfig, ILog log, IProgramOptions batchOptions)
        {
            DbConfig      = dbConfig;
            Log           = log;
            ProgramCode   = batchOptions.ProgramCode;
            Sproc         = batchOptions.Sproc;
            _isSimulation = batchOptions.IsSimulation;
        }

        public void RunSproc(string name, string sproc = null, SqlParameter[] parameters = null)
        {
            Sproc = sproc ?? $"wwp.{Sproc}";
            Log.Info($"Running job {name}");
            DataTable = new DataTable();

            using (var cn = new SqlConnection(DbConfig.ConnectionString))
            {
                var cmd = cn.CreateCommand();
                cmd.CommandText    = Sproc;
                cmd.CommandType    = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;

                if (_isSimulation)
                {
                    var sqlParm = new SqlParameter
                                  {
                                      ParameterName = "@Debug",
                                      Direction     = ParameterDirection.Input,
                                      Value         = true,
                                      DbType        = DbType.Boolean
                                  };

                    cmd.Parameters.Add(sqlParm);
                }

                if (parameters?.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                Log.Debug($"\tcalling {DbConfig.Catalog}.{Sproc}");

                if (cmd.Parameters.Count > 0)
                {
                    for (var idx = 0; idx < cmd.Parameters.Count; idx++)
                    {
                        var p = cmd.Parameters[idx];
                        Log.Debug($"\twith parm {p}: {p.Value.ToString().ToLower()}");
                    }
                }
                else
                {
                    Log.Debug("\twith no parms");
                }

                cn.Open();

                using (var da = new SqlDataAdapter(cmd))
                {
                    var numRows = da.Fill(DataTable);
                    Log.Debug($"\t{Sproc} returned ({numRows}) rows in result set");
                }
            }
        }

        public DataTable ExecSql(string sql)
        {
            var dataTable = new DataTable();

            using (var cn = new SqlConnection(DbConfig.ConnectionString))
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        #endregion
    }
}
