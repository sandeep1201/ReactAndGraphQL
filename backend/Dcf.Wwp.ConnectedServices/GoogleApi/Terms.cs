using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "terms")]
    public  class Terms
    {
        [DataMember]
        public string value { get; set; }
    }
}