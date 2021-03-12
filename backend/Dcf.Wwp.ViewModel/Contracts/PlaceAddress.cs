using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PlaceAddress
    {
        public string StreetAddress { get; set; }
        public string AptUnit { get; set; }
        public string City         { get; set; }
        public string State        { get; set; }
        public string Country { get; set; }
        public string ZipCode      { get; set; }
    }
}
