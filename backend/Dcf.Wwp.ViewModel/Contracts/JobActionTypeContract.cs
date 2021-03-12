using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class JobActionTypeContract
    {
        public JobActionTypeContract()
        {
            JobActionTypes = new List<int>();      
            JobActionNames = new List<string>();     
        }

        [DataMember(Name = "JobActionTypes")]
        public List<int> JobActionTypes { get; set; }

        [DataMember(Name = "JobActionNames")]
        public List<string> JobActionNames { get; set; }
    }
}