using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts.Cww
{
    public class SocialSecurityStatus
    {
        [DataMember(Name = "participant")]
        public string Participant { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "middle")]
        public string Middle { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "dob")]
        public DateTime? Dob { get; set; }

        [DataMember(Name = "relationship")]
        public string Relationship { get; set; }

        [DataMember(Name = "age")]
        public string Age { get; set; }

        [DataMember(Name = "fedSsi")]
        public string FedSsi { get; set; }

        [DataMember(Name = "stateSsi")]
        public string StateSsi { get; set; }

        [DataMember(Name = "ssa")]
        public string Ssa { get; set; }
    }
}
