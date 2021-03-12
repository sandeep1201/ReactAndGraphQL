using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using Dcf.Wwp.Batch.Interfaces;
using Newtonsoft.Json;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP21 : IBatchJob
    {
        #region Properties

        private readonly IBaseJob               _baseJob;
        private readonly IWwpPathConfig         _wwpPathConfig;
        private readonly IHttpWebRequestWrapper _httpWebRequest;

        public string Name    => GetType().Name;
        public string Desc    => "Updated ParticipantPlacement in WWP from T3048_W2_PLCM_WWP";
        public string Sproc   => "wwp.USP_DB2_T3048_Read_Update";
        public int    NumRows => 0;

        #endregion

        #region Methods

        public JWWWP21(IBaseJob baseJob, IWwpPathConfig wwpPathConfig, IHttpWebRequestWrapper httpWebRequest)
        {
            _baseJob        = baseJob;
            _wwpPathConfig  = wwpPathConfig;
            _httpWebRequest = httpWebRequest;
        }

        public DataTable Run()
        {
            try
            {
                GetOrUpdateUnProcessedPlacementEntries();

                if (_baseJob.DataTable.Rows.Count > 0)
                {
                    var key = UpdateBatchKey();

                    CallUpdatePlacementWebService(key);
                }
            }
            catch (SqlException ex)
            {
                _baseJob.Log.Error(ex.Errors.Count > 0 ? JsonConvert.SerializeObject(ex.Errors) : ex.InnerException?.Message);
                throw;
            }
            catch (WebException ex)
            {
                using (var response = ex.Response)
                {
                    if (response != null)
                    {
                        using (var sr = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                        {
                            var res = sr.ReadToEnd();

                            _baseJob.Log.Error(string.IsNullOrWhiteSpace(res) ? $"{ex.Message}" : $"{res}");
                        }
                    }
                    else
                        _baseJob.Log.Error(ex.InnerException == null ? $"{ex.Message}" : $"{ex.InnerException.Message}");
                }

                throw;
            }
            catch (Exception ex)
            {
                _baseJob.Log.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }
            finally
            {
                UpdateBatchKey(false);
            }

            return _baseJob.DataTable;
        }

        private void GetOrUpdateUnProcessedPlacementEntries(bool isUpdate = false)
        {
            _baseJob.RunSproc(Name, Sproc, new[]
                                           {
                                               new SqlParameter
                                               {
                                                   ParameterName = "@IsUpdate",
                                                   Direction     = ParameterDirection.Input,
                                                   Value         = isUpdate,
                                                   DbType        = DbType.Boolean
                                               }
                                           });
        }

        private string UpdateBatchKey(bool setKey = true)
        {
            const string tableName = "wwp.SpecialInitiative";

            var key        = setKey ? Guid.NewGuid().ToString() : "null";
            var setKeyStmt = setKey ? $"'{key}'" : "null";

            _baseJob.Log.Info($"Updating {tableName} w/{(setKey ? "key" : "null")}");

            _baseJob.ExecSql($"UPDATE {tableName} SET ParameterValue = {setKeyStmt}, ModifiedBy ='WWP Batch', ModifiedDate = '{DateTime.Now}' WHERE ParameterName = 'BatchKey'");

            return key;
        }

        private void CallUpdatePlacementWebService(string key)
        {
            var requestJson = JsonConvert.SerializeObject(_baseJob.DataTable);
            var postBytes   = Encoding.UTF8.GetBytes(requestJson);

            _httpWebRequest.HttpWebRequest = (HttpWebRequest) WebRequest.Create($"{_wwpPathConfig.Path}api\\update-placement\\{key}");

            _httpWebRequest.ContentLength = postBytes.Length;
            _httpWebRequest.ContentType   = "application/json";
            _httpWebRequest.Method        = "PUT";

            using (var streamWriter = new StreamWriter(_httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(requestJson);
                streamWriter.Flush();
                streamWriter.Close();
            }

            _baseJob.Log.Info("Calling Update Placement WebService");

            using (_httpWebRequest.GetResponse())
            {
                _baseJob.Log.Info("Update Placement WebService Executed Successfully");

                GetOrUpdateUnProcessedPlacementEntries(true);
            }
        }
    }

    #endregion
}
