using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts.Cww
{
    public class ChildCareEligibility
    {
        [DataMember(Name = "eligibilityStatus")]
        public string EligibilityStatus { get; set; }

        [DataMember(Name = "reasonCode")]
        public string ReasonCode { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "reasonCode1")]
        public string ReasonCode1 { get; set; }

        [DataMember(Name = "description1")]
        public string Description1 { get; set; }

        [DataMember(Name = "reasonCode2")]
        public string ReasonCode2 { get; set; }

        [DataMember(Name = "description2")]
        public string Description2 { get; set; }
    }
}
