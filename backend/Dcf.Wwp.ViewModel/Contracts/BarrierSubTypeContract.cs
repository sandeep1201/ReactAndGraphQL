using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class BarrierSubTypeContract
    {
        public BarrierSubTypeContract()
        {
            BarrierSubTypes = new List<int>();
            BarrierSubTypeNames = new List<string>();
        }

        [DataMember(Name = "JobActionTypes")]
        public List<int> BarrierSubTypes { get; set; }

        [DataMember(Name = "JobActionNames")]
        public List<string> BarrierSubTypeNames { get; set; }
    }
}
