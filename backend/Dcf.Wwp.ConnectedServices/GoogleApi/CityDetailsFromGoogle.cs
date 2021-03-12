using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract]
    public class CityDetailsFromGoogle
    {
        [DataMember]
        public object[] html_attributions { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public Result result { get; set; }
    }
}
