using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Cww
{
   public interface ISocialSecurityStatus
    {
         String Participant { get; set; }
         String FirstName { get; set; }
         String Middle { get; set; }
         String LastName { get; set; }
         DateTime? Dob { get; set; }
         String Relationship { get; set; }
         String Age { get; set; }
         String FedSsi { get; set; }
         String StateSsi { get; set; }
         String Ssa { get; set; }
    }
}
