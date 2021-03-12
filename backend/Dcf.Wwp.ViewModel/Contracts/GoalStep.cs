using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    class GoalStep
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int GoalId { get; set; }
        []
    }
}
