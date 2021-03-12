using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class PreRfaResponseContract
    {
        [DataMember(Name = "status")]
        public bool CanEnroll        { get; set; }

        [DataMember(Name = "errors")]
        public List<string> Errors   { get; set; }

        [DataMember(Name = "warnings")]
        public List<string> Warnings { get; set; }

        public PreRfaResponseContract()
        {
            Warnings = new List<string>();
            Errors   = new List<string>();
        }
    }
}
