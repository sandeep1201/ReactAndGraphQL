using System;
using System.Data;
using System.Data.SqlClient;
using Dcf.Wwp.Batch.Interfaces;
using Newtonsoft.Json;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP09 : IBatchJob
    {
        #region Properties

        private readonly IBaseJob _baseJob;

        public string Name    => GetType().Name;
        public string Desc    => "Copy Data from DB2 T0485 to SQL Server T0485";
        public string Sproc   => "wwp.USP_Copy_T0485_Data";
        public int    NumRows => 0;

        #endregion

        #region Methods

        public JWWWP09(IBaseJob baseJob)
        {
            _baseJob = baseJob;
        }

        public DataTable Run()
        {
            try
            {
                _baseJob.RunSproc(Name, Sproc);
            }
            catch (SqlException ex) // Catch the error message from sql server to log it
            {
                _baseJob.Log.Error(ex.Errors.Count > 0 ? JsonConvert.SerializeObject(ex.Errors) : ex.InnerException?.Message);
                throw;
            }
            catch (Exception ex)
            {
                _baseJob.Log.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }


            return _baseJob.DataTable;
        }
    }

    #endregion
}
