using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using log4net;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP05 : IBatchJob
    {
        #region Properties

        private readonly IDbConfig _dbConfig;
        private readonly ILog      _log;

        public string Name  => GetType().Name;
        public string Desc  => "Deactivate Users with no Activity in Past 6 Months";
        public string Sproc => "";

        public int NumRows { get; private set; }

        #endregion

        #region Methods

        public JWWWP05(IDbConfig dbConfig, ILog log)
        {
            _dbConfig = dbConfig;
            _log      = log;
        }

        public DataTable Run()
        {
            _log.Info($"Running job {Name}");
            _log.Debug($"\t------------------------------------------------------------------------------------");
            var dataTable = new DataTable(); // the tabled returned for exporting
            dataTable.Columns.Add("Id",                     typeof(int));
            dataTable.Columns.Add("WAMSId",                 typeof(string));
            dataTable.Columns.Add("WorkerName",             typeof(string));
            dataTable.Columns.Add("WorkerActiveStatusCode", typeof(string));
            dataTable.Columns.Add("LastLogin",              typeof(DateTime));


            var sqlSelectWkr = @"SELECT Id, WAMSId, RTRIM(LTRIM(FirstName + ' ' + LastName)) WorkerName, WorkerActiveStatusCode, LastLogin
                                 FROM wwp.Worker
                                 WHERE (LastLogin < DATEADD(MONTH, -6, GETDATE()) OR LastLogin IS NULL) AND WorkerActiveStatusCode NOT LIKE 'INACTIVE'";
            var sqlWkrTaskCat = @"SELECT *
                                  FROM wwp.WorkerTaskStatus";

            try
            {
                using (var cn = new SqlConnection(_dbConfig.ConnectionString))
                {
                    // read the Worker table
                    var dtWkr       = ExecSql(cn, sqlSelectWkr);
                    var wkrTaskStat = ExecSql(cn, sqlWkrTaskCat);
                    var statuses    = new List<string> { "OP", "CL" };
                    var wkrTaskStatuses = wkrTaskStat.AsEnumerable()
                                                     .Where(i => statuses.Contains(i.Field<string>("Code")))
                                                     .ToLookup(i => new { Id = i.Field<int>("Id"), Code = i.Field<string>("Code") });
                    var openId  = wkrTaskStatuses.Where(i => i.Key.Code == "OP").Select(i => i.Key.Id).First();
                    var closeId = wkrTaskStatuses.Where(i => i.Key.Code == "CL").Select(i => i.Key.Id).First();

                    foreach (DataRow wkr in dtWkr.Rows)
                    {
                        var wkrId        = wkr["Id"];
                        var wamsId       = wkr["WamsId"];
                        var workerName   = wkr["WorkerName"];
                        var lastLogin    = wkr["LastLogin"];
                        var updateWkrSql = $"UPDATE wwp.Worker SET WorkerActiveStatusCode = 'INACTIVE', ModifiedBy ='WWP Batch', ModifiedDate = '{DateTime.Now}' WHERE Id = {wkrId}";
                        var updateWkrTaskSql = "UPDATE wwp.WorkerTaskList "                                                                     +
                                               $"SET WorkerTaskStatusId = {closeId}, ModifiedBy ='WWP Batch', ModifiedDate = '{DateTime.Now}' " +
                                               $"WHERE WorkerId = {wkrId} "                                                                     +
                                               $"AND WorkerTaskStatusId = {openId}";

                        // Update Worker Table
                        ExecSql(cn, updateWkrSql);
                        ExecSql(cn, updateWkrTaskSql);

                        // add to exporting table
                        var newRow = dataTable.NewRow();
                        newRow["Id"]                     = wkrId;
                        newRow["WamsId"]                 = wamsId;
                        newRow["WorkerName"]             = workerName;
                        newRow["WorkerActiveStatusCode"] = "INACTIVE";
                        newRow["LastLogin"]              = lastLogin;
                        dataTable.Rows.Add(newRow);
                    }

                    NumRows = dataTable.Rows.Count;
                    _log.Debug($"\t{Sproc} returned ({NumRows}) rows in result set");
                }
            }
            catch (SqlException ex)
            {
                _log.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }


            return dataTable;
        }

        private DataTable ExecSql(SqlConnection cn, string sql)
        {
            var dataTable = new DataTable();

            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;


                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dataTable);
                }
            }

            return (dataTable);
        }
    }

    #endregion
}
