using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface.Cww;

namespace Dcf.Wwp.Model.Cww
{
    public class FsetStatus:IFsetStatus
    {
       public string CurrentStatusCode { get; set; }
       public DateTime? EnrollmentDate { get; set; }
       public DateTime? DisenrollmentDate { get; set; }
       public string DisenrollmentReasonCode { get; set; }
    }
}
