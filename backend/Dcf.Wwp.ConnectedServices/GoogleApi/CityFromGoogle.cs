using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    // http://jsontodatacontract.azurewebsites.net/

    // Type created for JSON at <<root>>
    [DataContract]
    public  class CityFromGoogle
    {
        [DataMember]
        public Predictions[] predictions { get; set; }

        [DataMember]
        public string status { get; set; }
    }
}
