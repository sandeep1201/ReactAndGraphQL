using System;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.Timelimits;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ChangeReasonContract : BaseModelContract
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public Boolean IsRequired { get; set; }

        [DataMember]
        public String Code { get;set; }
    }
}