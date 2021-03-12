using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Cww
{
   public interface IFsetStatus
    {
        String CurrentStatusCode { get; set; }
        DateTime? EnrollmentDate { get; set; }
        DateTime? DisenrollmentDate { get; set; }
        String DisenrollmentReasonCode { get; set; }
    }
}
