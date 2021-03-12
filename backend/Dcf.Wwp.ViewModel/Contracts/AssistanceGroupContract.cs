using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class AssistanceGroupContract
    {
        public List<AssistanceGroupMemberContract> Parents { get; set; } = new List<AssistanceGroupMemberContract>();
        public List<AssistanceGroupMemberContract> Children { get; set; } = new List<AssistanceGroupMemberContract>();

    }

    [DataContract]
    public class AssistanceGroupMemberContract 
    {
        [DataMember]
        public decimal Pin { get; set; }
        [DataMember]
        public Boolean IsSelectable { get; set; }
        [DataMember]
        public String Relationship { get; set; }
        [DataMember]
        public Boolean IsPlaced { get; set; }


        public static AssistanceGroupMemberContract Create(Decimal pin, Boolean isSelectable, String relationship, Boolean? isPlacedParent)
        {

            var agContract = new AssistanceGroupMemberContract
            {
                Pin = pin,
                IsSelectable = isSelectable,
                Relationship = relationship,
                IsPlaced = isPlacedParent.GetValueOrDefault()

            };
            return agContract;

        }

    }
}