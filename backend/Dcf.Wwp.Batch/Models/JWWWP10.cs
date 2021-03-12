using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dcf.Wwp.Batch.Infrastructure;
using Dcf.Wwp.Batch.Interfaces;
using Newtonsoft.Json;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP10 : IBatchJob
    {
        #region Properties

        private readonly IBaseJob               _baseJob;
        private readonly IOverUnderPaymentEmail _overUnderPaymentEmail;

        public string Name    => GetType().Name;
        public string Desc    => "Get T3018 Data After the BI Batch on the Night of Pull Down";
        public string Sproc   => "wwp.USP_Get_T3018_Data";
        public int    NumRows => 0;

        #endregion

        #region Methods

        public JWWWP10(IBaseJob baseJob, IOverUnderPaymentEmail overUnderPaymentEmail)
        {
            _baseJob               = baseJob;
            _overUnderPaymentEmail = overUnderPaymentEmail;
        }

        public DataTable Run()
        {
            try
            {
                _baseJob.RunSproc(Name, Sproc);

                var overPayments = _baseJob.DataTable.ConvertDataTable<OverUnderPaymentResult>()
                                           .Where(i => i.WorkerId == null && i.RevisedPaymentAmount != 0)
                                           .ToList();

                if (overPayments.Any())
                    _overUnderPaymentEmail.SendEmail(_baseJob.DbConfig.Catalog, overPayments);
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
