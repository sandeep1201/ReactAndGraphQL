using System;
using System.IO;
using System.Net;
using Dcf.Wwp.BritsBatch.Interfaces;
using log4net;
using Newtonsoft.Json;

namespace Dcf.Wwp.BritsBatch.Models
{
    public class EntSecService : IEntSecService
    {
        #region Properties

        private readonly ILog          _log;
        private readonly IEntSecConfig _esConfig;

        #endregion

        #region Methods

        public EntSecService (ILog log, IEntSecConfig esConfig)
        {
            _log      = log;
            _esConfig = esConfig;
        }

        private static HttpWebRequest httpReq { get; set; }

        public string GetToken()
        {
            string authToken = null;
            try
            {
                httpReq = (HttpWebRequest) WebRequest.Create(_esConfig.Endpoint);
                httpReq.Headers.Add("ApplicationKey", _esConfig.ApplicationKey);
                httpReq.Headers.Add("Username",       _esConfig.Username);
                httpReq.Headers.Add("Password",       _esConfig.Password);

                httpReq.Method        = "POST";
                httpReq.ContentLength = 0;

                using (var httpRes = (HttpWebResponse) httpReq.GetResponse())
                {
                    using (var rs = httpRes.GetResponseStream())
                    {
                        if (rs != null)
                        {
                            EnsSecResponse reply;
                            using (var sr = new StreamReader(rs))
                            {
                                var res = sr.ReadToEnd();

                                reply = JsonConvert.DeserializeObject<EnsSecResponse>(res);
                            }

                            var messageCode = reply.MessageCode;
                            _log.Debug($"\tAutnTokenMsgCode: {messageCode}");

                            if (messageCode == "SUCCESS")
                                authToken = reply.AuthenticatedToken;
                        }
                    }
                }

                return (authToken);
            }
            catch (InvalidOperationException ex)
            {
                _log.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _log.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }
        }

        #endregion
    }
}
