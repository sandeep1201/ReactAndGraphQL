using System.Data;
using System.Data.SqlClient;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP00 : IJWWWP00
    {
        #region Properties

        private readonly IDbConfig _dbConfig;

        public string Name    => GetType().Name;
        public string Desc    => "DB2 Auto Disenrollment Update";
        public string Sproc   => "wwp.SP_DB2_Auto_Disenrollment_Update";
        public int    NumRows { get; private set; }

        #endregion

        #region Methods

        public JWWWP00(IDbConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }

        public DataTable Run()
        {
            //TODO: Add logging & try/catch/finally

            var dataTable = new DataTable();

            using (var cn = new SqlConnection(_dbConfig.ConnectionString))
            {
                var cmd = cn.CreateCommand();
                cmd.CommandText    = Sproc;
                cmd.CommandType    = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;

                var parameter = new SqlParameter
                                {
                                    ParameterName = "@SchemaName", // this is really the catalog/database name, *not* the 'schema'
                                    Direction     = ParameterDirection.Input,
                                    Value         = _dbConfig.Catalog,
                                };

                cmd.Parameters.Add(parameter);

                cn.Open();

                using (var da = new SqlDataAdapter(cmd))
                {
                    NumRows = da.Fill(dataTable);
                }
            }

            return (dataTable);
        }

        #endregion
    }
}
