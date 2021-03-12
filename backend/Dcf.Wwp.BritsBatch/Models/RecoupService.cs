using System;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using Dcf.Wwp.BritsBatch.Interfaces;
using log4net;
using log4net.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dcf.Wwp.BritsBatch.Models
{
    public class RecoupService : IRecoupService
    {
        #region Properties

        private readonly IGetRecoupConfig _rcConfig;
        private readonly ILog             _log;

        #endregion

        #region Methods

        public RecoupService(IGetRecoupConfig rcConfig, ILog log)
        {
            _rcConfig = rcConfig;
            _log      = log;
        }

        private static HttpWebRequest httpReq { get; set; }

        public XDocument GetRecoupResponse(string getRecoupJson, bool isSimulation)
        {
            // turn our request string into a byte stream
            var postBytes = Encoding.UTF8.GetBytes(getRecoupJson);

            httpReq             = (HttpWebRequest) WebRequest.Create(_rcConfig.Endpoint);
            httpReq.ContentType = "application/json";

            GetRecoupResponse reply;
            string            rsXml;
            XDocument         xml;

            httpReq.Method        = "POST";
            httpReq.ContentLength = postBytes.Length;

            using (var streamWriter = new StreamWriter(httpReq.GetRequestStream()))
            {
                streamWriter.Write(getRecoupJson);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var httpRes = (HttpWebResponse) httpReq.GetResponse())
            {
                using (var sr = new StreamReader(httpRes.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    var res = sr.ReadToEnd();

                    reply = JsonConvert.DeserializeObject<GetRecoupResponse>(res);
                    if (isSimulation)
                        _log.Debug("*****************************************************************************************************************************************\nGetRecoupResponse: " +
                                   $"{JToken.Parse(res).ToString(Formatting.Indented)}\n");

                    if (reply.MessageCodes.Count == 1 && reply.MessageCodes[0] == "SUCCESS")
                    {
                        rsXml = JsonConvert.DeserializeXNode(res, "Root").ToString();
                        xml   = XDocument.Parse(rsXml);
                        var xel = xml.Descendants("WWCaseCalcRecoupmentInfoList").ToList();

                        foreach (var xe in xel)
                        {
                            var xeLiablePINElements   = xe.Descendants("LiablePINList").ToList();
                            var joinedValues          = string.Join(",", xeLiablePINElements.Select(xec => xec.Value));
                            var newXeLiablePINElement = new XElement("LiablePINList", joinedValues);
                            xeLiablePINElements.Remove();
                            xe.Add(newXeLiablePINElement);
                        }
                    }
                    else
                    {
                        _log.ErrorExt("*****************************************************************************************************************************************\n" +
                                      "Response MessageCode is not SUCCESS\n"                                                                                                       +
                                      "*****************************************************************************************************************************************");
                        throw new InvalidMessageContractException();
                    }
                }
            }

            return (xml);
        }

        public PostRecoupResponse PostRecoupResponse(string postRecoupJson, bool isSimulation)
        {
            // turn our request string into a byte stream
            var postBytes = Encoding.UTF8.GetBytes(postRecoupJson);

            httpReq             = (HttpWebRequest) WebRequest.Create(_rcConfig.PostEndpoint);
            httpReq.ContentType = "application/json";

            PostRecoupResponse reply;

            httpReq.Method        = "POST";
            httpReq.ContentLength = postBytes.Length;

            using (var streamWriter = new StreamWriter(httpReq.GetRequestStream()))
            {
                streamWriter.Write(postRecoupJson);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var httpRes = (HttpWebResponse) httpReq.GetResponse())
            {
                using (var sr = new StreamReader(httpRes.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    var res = sr.ReadToEnd();

                    reply = JsonConvert.DeserializeObject<PostRecoupResponse>(res);
                    if (isSimulation)
                        _log.Debug("*****************************************************************************************************************************************\nGetRecoupResponse: " +
                                   $"{JToken.Parse(res).ToString(Formatting.Indented)}\n");

                    if (reply.MessageCodes.Count == 1 && reply.MessageCodes[0] == "SUCCESS")
                    {
                    }

                    else
                    {
                        _log.ErrorExt("*****************************************************************************************************************************************\n" +
                                      "Response MessageCode is not SUCCESS\n"                                                                                                       +
                                      "*****************************************************************************************************************************************");
                        throw new InvalidMessageContractException();
                    }
                }
            }

            return (reply);
        }

        #endregion
    }
}
