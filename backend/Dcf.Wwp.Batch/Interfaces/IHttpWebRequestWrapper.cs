using System;
using System.IO;
using System.Net;

namespace Dcf.Wwp.Batch.Interfaces
{
    public interface IHttpWebRequestWrapper
    {
        HttpWebRequest          HttpWebRequest { get; set; }
        Stream                  GetRequestStream();
        IHttpWebResponseWrapper GetResponse();
        long                    ContentLength { get; set; }
        string                  ContentType   { get; set; }
        string                  Method        { get; set; }
        Uri                     RequestUri    { get; }
    }
}
