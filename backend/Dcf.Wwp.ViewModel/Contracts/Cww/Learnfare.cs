using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts.Cww
{
    public class Learnfare
    {
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "middleInitial")]
        public string MiddleInitial { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "dateOfBirth")]
        public string BirthDate { get; set; }

        [DataMember(Name = "learnFareStatus")]
        public string LearnFareStatus { get; set; }
    }
}
