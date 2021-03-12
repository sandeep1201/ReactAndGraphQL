using System.Collections.Generic;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    public class GoogleData : IGoogleData
    {
        public List<IGoogleCity>          Cities          { get; set; }
        public List<IGooglePlace>         Schools         { get; set; }
        public List<IGoogleStreetAddress> StreetAddresses { get; set; }
        public IGoogleStreetAddress       Address         { get; set; }
    }
}
