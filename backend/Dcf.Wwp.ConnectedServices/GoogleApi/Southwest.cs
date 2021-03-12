using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "southwest")]
    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}