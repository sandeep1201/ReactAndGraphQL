using System;
using System.Data;
using System.Data.SqlClient;
using Dcf.Wwp.Batch.Interfaces;
using Dcf.Wwp.Batch.UnitTests.Models;
using log4net;

namespace Dcf.Wwp.Batch.UnitTests.Infrastructure
{
    public class MockBaseJob : IBaseJob
    {
        #region Properties

        public IDbConfig DbConfig              { get; } = null;
        public ILog      Log                   { get; } = LogManager.GetLogger(typeof(MockBaseJob));
        public string    ProgramCode           { get; } = null;
        public DataTable DataTable             { get; } = new DataTable();
        public string    JobName               { get; set; }
        public int       SprocReturnCount      { get; set; } = 1;
        public int       SprocRunCount         { get; private set; }
        public int       SqlRunCount           { get; private set; }
        public bool      HasRunSprocBeenCalled { get; private set; }
        public bool      HasExecSqlBeenCalled  { get; private set; }

        #endregion Properties

        #region Methods

        public void RunSproc(string name, string sproc = null, SqlParameter[] parameters = null)
        {
            SprocRunCount         += 1;
            HasRunSprocBeenCalled =  true;
            if (JobName == nameof(JWWWP21Test))
            {
                DataTable.Columns.Clear();
                DataTable.Columns.AddRange(new[]
                                           {
                                               new DataColumn("CaseNumber",         typeof(decimal)),
                                               new DataColumn("PinNumber",          typeof(decimal)),
                                               new DataColumn("PlacementType",      typeof(string)),
                                               new DataColumn("PlacementStartDate", typeof(DateTime)),
                                               new DataColumn("PlacementEndDate",   typeof(DateTime)),
                                               new DataColumn("MFFEPId",            typeof(string))
                                           });

                DataTable.Rows.Clear();

                if (SprocReturnCount > 0)
                {
                    DataTable.Rows.Add();
                    DataTable.Rows[0]["CaseNumber"]         = 9110035591;
                    DataTable.Rows[0]["PinNumber"]          = 9524835321;
                    DataTable.Rows[0]["PlacementType"]      = "CSJ";
                    DataTable.Rows[0]["PlacementStartDate"] = DateTime.Parse("2020-10-13");
                    DataTable.Rows[0]["PlacementEndDate"]   = DateTime.Parse("2021-02-28");
                    DataTable.Rows[0]["MFFEPId"]            = "DCFFEP";
                }
            }
        }

        public DataTable ExecSql(string sql)
        {
            SqlRunCount          += 1;
            HasExecSqlBeenCalled =  true;

            return new DataTable();
        }

        #endregion
    }
}
