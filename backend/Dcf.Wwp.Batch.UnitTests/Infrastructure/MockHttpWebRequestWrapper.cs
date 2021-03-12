using System;
using System.IO;
using System.Net;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.UnitTests.Infrastructure
{
    public class MockHttpWebRequestWrapper : IHttpWebRequestWrapper
    {
        #region Properties

        public HttpWebRequest HttpWebRequest                { get; set; }
        public long           ContentLength                 { get; set; }
        public string         ContentType                   { get; set; }
        public string         Method                        { get; set; }
        public bool           HasGetRequestStreamBeenCalled { get; private set; }
        public bool           HasGetResponseBeenCalled      { get; private set; }

        #endregion

        #region Methods

        public Stream GetRequestStream()
        {
            HasGetRequestStreamBeenCalled = true;
            return new MemoryStream();
        }

        public IHttpWebResponseWrapper GetResponse()
        {
            HasGetResponseBeenCalled = true;
            return null;
        }

        public Uri RequestUri => HttpWebRequest.RequestUri;

        #endregion
    }
}
