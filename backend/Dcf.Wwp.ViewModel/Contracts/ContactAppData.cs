using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
	[DataContract]
    public class ContactAppData
    {
        [DataMember(Name = "pin")]
        public string Pin { get; set; }

        [DataMember(Name = "contacts")]
        public List<ContactContract> Contacts { get; set; }
    }
}
