
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class DemographicSearchContract : DemographicResultContract
    {
        public bool HasNoSsn { get; set; }
        public List<PersonNameContract> Aliases { get; set; }
    }
}
