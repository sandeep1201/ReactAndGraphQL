using System.Collections.Generic;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class GoogleData : IGoogleData
    {
        public List<IGoogleCity> Cities { get; set; }

        //public List<GoogleCity> UsCities { get; set; }

        public List<IGooglePlace> Schools { get; set; }

        //public List<GoogleCollege> Colleges { get; set; }

        //public List<GoogleEstablishment> Establishments { get; set; }

        public List<IGoogleStreetAddress> StreetAddresses { get; set; }

        public IGoogleStreetAddress Address { get; set; }
    }
}
