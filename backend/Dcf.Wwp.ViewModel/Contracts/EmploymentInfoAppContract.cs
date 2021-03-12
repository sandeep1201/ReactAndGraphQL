using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class EmploymentInfoAppContract
    {
        [DataMember(Name = "pin")]
        public string Pin { get; set; }

        [DataMember(Name = "employments")]
        public List<EmploymentInfoContract> Employments { get; set; }
    }
}
