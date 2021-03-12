using System;
using System.IO;
using System.Linq;
using Dcf.Wwp.BritsBatch.Interfaces;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dcf.Wwp.BritsBatch.Models
{
    public class JWBWP01 : IBatchJob
    {
        #region Properties

        private readonly ILog            _log;
        private readonly IRecoupAmtSproc _recoupAmtSproc;
        private readonly IEntSecService  _entSec;
        private readonly IRecoupService  _getRecoup;
        private readonly IProgramOptions _options;

        public string Name    => GetType().Name;
        public string Desc    => "WW Recoupment Amount";
        public string Sproc   => "wwp.USP_PostWWRecoupment_Response"; //call the actual SP
        public int    NumRows { get; private set; }

        #endregion

        #region Methods

        public JWBWP01(ILog log, IRecoupAmtSproc recoupAmtSproc, IEntSecService entSec, IRecoupService getRecoup, IProgramOptions options)
        {
            _log            = log;
            _recoupAmtSproc = recoupAmtSproc;
            _entSec         = entSec;
            _getRecoup      = getRecoup;
            _options        = options;
        }

        public string Run()
        {
            _log.Info($"Running job {Name}");

            try
            {
                var recoupmentCaseInfoList = _recoupAmtSproc.ExecPostSproc();

                if (recoupmentCaseInfoList != null)
                {
                    var authToken = _entSec.GetToken();

                    if (authToken != null)
                    {
                        var tokenJson      = "{\"AuthenticatedToken\": "               + JsonConvert.SerializeObject(authToken);
                        var postRecoupJson = tokenJson + ", " + recoupmentCaseInfoList + "}";
                        if (_options.IsSimulation)
                            _log.Debug("*****************************************************************************************************************************************\nPostRecoupRequestBody: " +
                                       $"{JToken.Parse(postRecoupJson).ToString(Formatting.Indented)}\n");

                        var rsReply = _getRecoup.PostRecoupResponse(postRecoupJson, _options.IsSimulation);

                        if (rsReply != null)
                        {
                            var caseInfoJson = JsonConvert.DeserializeObject<PostRecoupRequest>("{" + recoupmentCaseInfoList + "}");
                            NumRows = caseInfoJson.PostRecoupmentList.Count;
                            var reqCount = rsReply.RequestCount;

                            if (rsReply.InvalidCaseList != null && rsReply.InvalidCaseList.Count != 0)
                            {
                                _log.Warn("*****************************************************************************************************************************************\n" +
                                          $"Response has following Invalid Cases: {JToken.Parse(JsonConvert.SerializeObject(rsReply.InvalidCaseList)).ToString(Formatting.Indented)}\n" +
                                          "*****************************************************************************************************************************************");
                                CountMatch(NumRows, reqCount);
                            }
                            else
                                CountMatch(NumRows, reqCount);
                        }
                    }
                    else
                    {
                        _log.Fatal("*****************************************************************************************************************************************\n" +
                                   "AuthToken is NULL\n"                                                                                                                         +
                                   "*****************************************************************************************************************************************");
                        throw new UnauthorizedAccessException();
                    }
                }
                else
                {
                    _log.Fatal("*****************************************************************************************************************************************\n" +
                               "RecoupmentCaseInfoList returned NULL\n"                                                                                                      +
                               "*****************************************************************************************************************************************");
                }
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }

            return ("");
        }

        private void CountMatch(int numRows, int reqCount)
        {
            if (numRows != reqCount)
                _log.Warn("*****************************************************************************************************************************************\n" +
                          "Request: "                                                                                                                                   + numRows + " and Response: " + reqCount + " count did not match\n" +
                          "*****************************************************************************************************************************************");
            else
                _log.Info("Request: " + numRows + " and Response: " + reqCount + " count matched");
        }

        #endregion
    }
}
