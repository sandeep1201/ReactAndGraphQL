using System;
using System.IO;
using System.Net;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public class HttpWebRequestWrapper : IHttpWebRequestWrapper
    {
        #region Properties

        public HttpWebRequest HttpWebRequest { get; set; }

        #endregion

        #region Methods

        public Stream GetRequestStream()
        {
            return HttpWebRequest.GetRequestStream();
        }

        public IHttpWebResponseWrapper GetResponse()
        {
            return new HttpWebResponseWrapper(HttpWebRequest.GetResponse());
        }

        public long ContentLength
        {
            get => HttpWebRequest.ContentLength;
            set => HttpWebRequest.ContentLength = value;
        }

        public string ContentType
        {
            get => HttpWebRequest.ContentType;
            set => HttpWebRequest.ContentType = value;
        }

        public string Method
        {
            get => HttpWebRequest.Method;
            set => HttpWebRequest.Method = value;
        }

        public Uri RequestUri => HttpWebRequest.RequestUri;

        #endregion
    }
}
