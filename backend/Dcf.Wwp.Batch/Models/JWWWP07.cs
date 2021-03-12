using System;
using System.Data;
using System.Data.SqlClient;
using Dcf.Wwp.Batch.Interfaces;
using Newtonsoft.Json;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP07 : IBatchJob
    {
        #region Properties

        private readonly IBaseJob _baseJob;

        public string Name    => GetType().Name;
        public string Desc    => "Create new entries for WW and CF on the participation tracking table for the activities that are scheduled in the next participation period";
        public string Sproc   => "wwp.USP_Create_Participation_Entries";
        public int    NumRows => 0;

        #endregion

        #region Methods

        public JWWWP07(IBaseJob baseJob)
        {
            _baseJob = baseJob;
        }

        public DataTable Run()
        {
            try
            {
                var parameters = new []
                                 {
                                     new SqlParameter
                                     {
                                         ParameterName = "@FromBatch",
                                         Direction     = ParameterDirection.Input,
                                         Value         = true,
                                         DbType        = DbType.Boolean
                                     },
                                     new SqlParameter
                                     {
                                         ParameterName = "@ProgramCd",
                                         Direction     = ParameterDirection.Input,
                                         Value         = _baseJob.ProgramCode,
                                         DbType        = DbType.String
                                     }
                                 };


                _baseJob.RunSproc(Name, Sproc, parameters);

                return _baseJob.DataTable;
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
        }

        #endregion
    }
}
