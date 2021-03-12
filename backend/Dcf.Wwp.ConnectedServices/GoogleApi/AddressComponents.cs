using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "address_components")]
    public  class AddressComponents
    {
        [DataMember]
        public string long_name { get; set; }

        [DataMember]
        public string short_name { get; set; }

        [DataMember]
        public string[] types { get; set; }
    }
}
