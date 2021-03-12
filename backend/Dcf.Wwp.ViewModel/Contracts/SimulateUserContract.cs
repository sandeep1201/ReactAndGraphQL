using System;
using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class SimulateUserContract
    {
        [DataMember]
        public string AuthorizedUserToken { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string[] Roles { get; set; }

        [DataMember]
        public string OrgCode { get; set; }

        [DataMember]
        public DateTime? CDODate { get; set; }
    }
}
