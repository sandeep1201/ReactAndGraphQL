using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
   public class GoogleEstablishment
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "streetAddress")]
        public string StreetAddress { get; set; }

        [DataMember(Name = "placeId")]
        public string PlaceId { get; set; }
    }
}
