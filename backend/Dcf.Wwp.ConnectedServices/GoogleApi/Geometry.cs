using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "geometry")]
    public class Geometry
    {
        [DataMember] public AccessPoints[] access_points { get; set; }
        [DataMember] public Location1      location { get; set; }
    }
}
