using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAddressVerificationTypeLookup : ICommonModelFinal
    {
        string                                Name                      { get; set; }
        ICollection<IAlternateMailingAddress> AlternateMailingAddresses { get; set; }
        ICollection<IParticipantContactInfo>  ParticipantContactInfoes  { get; set; }
    }
}
