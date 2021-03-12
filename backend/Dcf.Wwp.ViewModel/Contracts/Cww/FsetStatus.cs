using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts.Cww
{
    public class FsetStatus
    {
        [DataMember(Name ="currentStatusCode")]
        public string CurrentStatusCode { get; set; }

        [DataMember(Name = "enrollmentDate")]
        public string EnrollmentDate { get; set; }

        [DataMember(Name = "disenrollmentDate")]
        public string DisenrollmentDate { get; set; }

        [DataMember(Name = "disenrollmentReasonCode")]
        public string DisenrollmentReasonCode { get; set; }

      
    }
}
