using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "access_points")]
    public  class AccessPoints
    {
        [DataMember]
        public Location location { get; set; }

        [DataMember]
        public string[] travel_modes { get; set; }
    }
}
