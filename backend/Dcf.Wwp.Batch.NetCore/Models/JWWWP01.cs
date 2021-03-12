using System.Data;
using System.Data.SqlClient;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP01 : IJWWWP01
    {
        #region Properties

        private readonly IDbConfig _dbConfig;

        public string Name    => GetType().Name;
        public string Desc    => "RFA System Denial";
        public string Sproc   => "wwp.SP_RFA_System_Denial";
        public int    NumRows { get; private set; }

        #endregion

        #region Methods

        public JWWWP01(IDbConfig dbConfig)
        {
            _dbConfig = dbConfig;
            NumRows   = 0;
        }

        public DataTable Run()
        {
            //TODO: Add logging & try/catch

            var dataTable = new DataTable();

            using (var cn = new SqlConnection(_dbConfig.ConnectionString))
            {
                var cmd = cn.CreateCommand();
                cmd.CommandText    = Sproc;
                cmd.CommandType    = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;

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
