using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class OfficeCountyContract
    {
        public short CountyNumber { get; set; }
        public short OfficeNumber { get; set; }
        public string WPGeoArea { get; set; }
    }
}
