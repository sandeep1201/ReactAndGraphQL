using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    // Type created for JSON at <<root>>
    [DataContract]
    public  class SchoolsFromGoogle
    {
        [DataMember]
        public object[] html_attributions { get; set; }

        [DataMember]
        public Results[] results { get; set; }

        [DataMember]
        public string status { get; set; }
    }
}
