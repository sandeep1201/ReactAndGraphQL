using System;
using System.IO;
using System.Linq;
using Dcf.Wwp.BritsBatch.Interfaces;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dcf.Wwp.BritsBatch.Models
{
    public class JWBWP00 : IBatchJob
    {
        #region Properties

        private readonly ILog            _log;
        private readonly IRecoupAmtSproc _recoupAmtSproc;
        private readonly IEntSecService  _entSec;
        private readonly IRecoupService  _getRecoup;
        private readonly IProgramOptions _options;

        public string Name    => GetType().Name;
        public string Desc    => "WW Recoupment Amount";
        public string Sproc   => "wwp.USP_GetWWRecoupment_Post"; //call the actual SP
        public int    NumRows { get; private set; }

        #endregion

        #region Methods

        public JWBWP00(ILog log, IRecoupAmtSproc recoupAmtSproc, IEntSecService entSec, IRecoupService getRecoup, IProgramOptions options)
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
                var recoupmentCaseInfoList = _recoupAmtSproc.ExecGetSproc();

                if (recoupmentCaseInfoList != null)
                {
                    var authToken = _entSec.GetToken();

                    if (authToken != null)
                    {
                        var tokenJson     = "{\"AuthenticatedToken\": "               + JsonConvert.SerializeObject(authToken);
                        var getRecoupJson = tokenJson + ", " + recoupmentCaseInfoList + "}";
                        if (_options.IsSimulation)
                            _log.Debug("*****************************************************************************************************************************************\nGetRecoupRequestBody: " +
                                       $"{JToken.Parse(getRecoupJson).ToString(Formatting.Indented)}\n");

                        var rsReply = _getRecoup.GetRecoupResponse(getRecoupJson, _options.IsSimulation);
                        if (rsReply != null)
                        {
                            var caseInfoJson = JsonConvert.DeserializeObject<RecoupmentCaseInfoList>("{" + recoupmentCaseInfoList + "}");
                            NumRows = caseInfoJson.WWRecoupmentCaseInfoList.Count;
                            var reqCount    = int.Parse(rsReply.Descendants().SingleOrDefault(i => i.Name.LocalName == "RequestCount")?.Value ?? "0");
                            var reqCaseNums = caseInfoJson.WWRecoupmentCaseInfoList.Select(i => i.CaseNumber).ToList();
                            var resCaseNums = rsReply.Descendants().Where(i => i.Name.LocalName == "CaseNumber").Select(i => Convert.ToInt64(i.Value)).ToList();
                            var caseMatch   = resCaseNums.Except(resCaseNums).ToList();
                            var caseString  = string.Join(", ", caseMatch.Select(i => i.ToString()).ToArray());

                            if (caseMatch.Any())
                            {
                                _log.Warn("*****************************************************************************************************************************************\n" +
                                          "Response has different CASE_NUMs than the ones sent in request\n"                                                                            +
                                          "CASE_NUMs: "                                                                                                                                 + caseString + "\n" +
                                          "*****************************************************************************************************************************************");
                                CountMatch(NumRows, reqCount);
                            }
                            else
                            {
                                _log.Info("All cases in the response are available in the request");
                                CountMatch(NumRows, reqCount);
                            }

                            _recoupAmtSproc.ExecResponseSproc(rsReply);
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
                    throw new InvalidDataException();
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
