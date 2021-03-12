using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
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
