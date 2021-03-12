using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "aspects")]
    public  class Aspects
    {
        [DataMember]
        public int rating { get; set; }

        [DataMember]
        public string type { get; set; }
    }
}
