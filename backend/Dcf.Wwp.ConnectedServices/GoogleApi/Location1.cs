using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "location")]
    public  class Location1
    {
        [DataMember]
        public decimal lat { get; set; }

        [DataMember]
        public decimal lng { get; set; }
    }
}
