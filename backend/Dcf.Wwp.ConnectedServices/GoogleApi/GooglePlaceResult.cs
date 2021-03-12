using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract]
    public class GooglePlaceResult
    {
        [DataMember]
        public object[] html_attributions { get; set; }

        [DataMember]
        public Result[] results { get; set; }

        [DataMember]
        public string status { get; set; }
    }
}
