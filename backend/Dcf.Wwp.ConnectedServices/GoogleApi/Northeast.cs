using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    [DataContract(Name = "northeast")]
    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}