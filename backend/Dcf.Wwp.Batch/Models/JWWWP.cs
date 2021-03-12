using System;
using System.Data;
using System.Data.SqlClient;
using Dcf.Wwp.Batch.Interfaces;
using Newtonsoft.Json;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP : IBatchJob
    {
        #region Properties

        private readonly IBaseJob _baseJob;

        public string Name    => GetType().Name;
        public string Desc    => "";
        public string Sproc   => "";
        public int    NumRows => 0;

        #endregion

        #region Methods

        public JWWWP(IBaseJob baseJob)
        {
            _baseJob = baseJob;
        }

        public DataTable Run()
        {
            try
            {
                _baseJob.RunSproc(Name);
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
