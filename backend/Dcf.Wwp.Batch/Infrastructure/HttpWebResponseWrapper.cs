using System.Net;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public class HttpWebResponseWrapper : IHttpWebResponseWrapper
    {
        #region Properties

        private readonly WebResponse _webResponse;

        #endregion

        #region Methods

        public HttpWebResponseWrapper(WebResponse webResponse)
        {
            _webResponse = webResponse;
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            _webResponse?.Dispose();
        }

        #endregion
    }
}
