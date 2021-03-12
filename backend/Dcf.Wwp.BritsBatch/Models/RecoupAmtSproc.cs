using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using Dcf.Wwp.BritsBatch.Interfaces;
using log4net;
using Newtonsoft.Json;

namespace Dcf.Wwp.BritsBatch.Models
{
    public class RecoupAmtSproc : IRecoupAmtSproc
    {
        #region Properties

        private readonly IDbConfig       _dbConfig;
        private readonly IProgramOptions _options;
        private readonly ILog            _log;

        public string GetSproc       => "wwp.USP_GetWWRecoupment_Post";      //call the actual SP
        public string RessponseSproc => "wwp.USP_GetWWRecoupment_Response";  //call the actual SP
        public string PostSproc      => "wwp.USP_PostWWRecoupment_Response"; //call the actual SP
        public int    NumRows        { get; private set; }

        #endregion

        #region Methods

        public RecoupAmtSproc(IDbConfig dbConfig, IProgramOptions options, ILog log)
        {
            _dbConfig = dbConfig;
            _options  = options;
            _log      = log;
        }

        public string ExecGetSproc()
        {
            _log.Info($"Running SProc {GetSproc}");

            var    dataTable              = new DataTable();
            string recoupmentCaseInfoList = null;

            try
            {
                using (var cn = new SqlConnection(_dbConfig.ConnectionString))
                {
                    var cmd = cn.CreateCommand();
                    cmd.CommandText    = GetSproc;
                    cmd.CommandType    = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;

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

                    _log.Debug($"\tcalling {_dbConfig.Catalog}.{GetSproc}");

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
                        _log.Debug($"\t{GetSproc} returned ({NumRows}) rows in result set");
                    }

                    if (NumRows > 0)
                    {
                        recoupmentCaseInfoList = JsonConvert.SerializeObject(dataTable);
                        recoupmentCaseInfoList = "\"WWRecoupmentCaseInfoList\": " + recoupmentCaseInfoList.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]");
                    }
                }
            }
            catch (SqlException ex)
            {
                _log.Fatal(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }

            return (recoupmentCaseInfoList);
        }

        public void ExecResponseSproc(XDocument xml)
        {
            _log.Info($"Running SProc {RessponseSproc}");

            var dataTable = new DataTable();

            try
            {
                using (var cn = new SqlConnection(_dbConfig.ConnectionString))
                {
                    var cmd = cn.CreateCommand();
                    cmd.CommandText    = RessponseSproc;
                    cmd.CommandType    = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;

                    var sqlXmlParam = new SqlParameter
                                      {
                                          ParameterName = "@XML",
                                          Direction     = ParameterDirection.Input,
                                          Value         = xml.ToString(),
                                          DbType        = DbType.Xml
                                      };
                    cmd.Parameters.Add(sqlXmlParam);

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

                    _log.Debug($"\tcalling {_dbConfig.Catalog}.{RessponseSproc}");

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
                        _log.Debug($"\t{_dbConfig.Catalog}.{RessponseSproc} executed successfully");
                    }
                }
            }
            catch (SqlException ex)
            {
                _log.Fatal(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }
        }

        public string ExecPostSproc()
        {
            _log.Info($"Running SProc {PostSproc}");

            var    dataTable              = new DataTable();
            string recoupmentCaseInfoList = null;

            try
            {
                using (var cn = new SqlConnection(_dbConfig.ConnectionString))
                {
                    var cmd = cn.CreateCommand();
                    cmd.CommandText    = PostSproc;
                    cmd.CommandType    = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;

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

                    _log.Debug($"\tcalling {_dbConfig.Catalog}.{PostSproc}");

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
                        _log.Debug($"\t{GetSproc} returned ({NumRows}) rows in result set");
                    }

                    if (NumRows > 0)
                    {
                        recoupmentCaseInfoList = JsonConvert.SerializeObject(dataTable);
                        recoupmentCaseInfoList = "\"PostRecoupmentList\": " + recoupmentCaseInfoList.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]");
                    }
                }
            }
            catch (SqlException ex)
            {
                _log.Fatal(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }

            return (recoupmentCaseInfoList);
        }

        #endregion
    }
}
