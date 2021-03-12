using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    // Type created for JSON at <<root>>
    [DataContract]
    public class EstablishmentsFromGoogle
    {
        #region Properties

        public object[]  html_attributions { get; set; }
        public string    next_page_token   { get; set; }
        public Results[] results           { get; set; }
        public string    status            { get; set; }

        #endregion
    }
}
