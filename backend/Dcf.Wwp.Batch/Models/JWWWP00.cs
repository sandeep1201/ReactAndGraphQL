using System.Data;
using System.Data.SqlClient;
using log4net;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP00 : IBatchJob
    {
        #region Properties

        private readonly IDbConfig       _dbConfig;
        private readonly IProgramOptions _options;
        private readonly ILog            _log;

        public string Name    => GetType().Name;
        public string Desc    => "DB2 Auto Disenrollment Update";
        public string Sproc   => "wwp.SP_DB2_Auto_Disenrollment_Update";
        public int    NumRows { get; private set; }

        #endregion

        #region Methods

        public JWWWP00(IDbConfig dbConfig, IProgramOptions options, ILog log)
        {
            _dbConfig = dbConfig;
            _options  = options;
            _log      = log;
        }

        public DataTable Run()
        {
            _log.Info($"Running job {Name}");

            var dataTable = new DataTable();

            try
            {
                using (var cn = new SqlConnection(_dbConfig.ConnectionString))
                {
                    var cmd = cn.CreateCommand();
                    cmd.CommandText    = Sproc;
                    cmd.CommandType    = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;

                    var sqlParmSchemaName = new SqlParameter
                                            {
                                                ParameterName = "@SchemaName", // this is really the catalog/database name, *not* the 'schema'
                                                Direction     = ParameterDirection.Input,
                                                Value         = _dbConfig.Catalog,
                                            };

                    cmd.Parameters.Add(sqlParmSchemaName);

                    if (_options.IsSimulation)
                    {
                        var sqlParmDebug = new SqlParameter
                                           {
                                               ParameterName = "@Debug",
                                               Direction     = ParameterDirection.Input,
                                               Value         = true,
                                               DbType        = DbType.Boolean
                                           };

                        cmd.Parameters.Add(sqlParmDebug);
                    }

                    _log.Debug($"\tcalling {_dbConfig.Catalog}.{Sproc}");

                    if (cmd.Parameters.Count > 0)
                    {
                        for (var idx = 0; idx < cmd.Parameters.Count; idx++)
                        {
                            var p = cmd.Parameters[idx];
                            _log.Debug($"\twith parm {p}: {p.Value}");
                        }
                    }
                    else
                    {
                        _log.Debug("\twith no parms");
                    }

                    cn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        NumRows = da.Fill(dataTable);
                        _log.Debug($"\t{Sproc} returned ({NumRows}) rows in resultset");
                    }
                }
            }
            catch (SqlException ex)
            {
                _log.Fatal(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }

            return (dataTable);
        }

        #endregion
    }
}
