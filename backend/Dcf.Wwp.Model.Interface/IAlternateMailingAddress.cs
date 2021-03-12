using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAlternateMailingAddress
    {
        string                               StreetAddressPlaceId            { get; set; }
        int?                                 StateId                         { get; set; }
        string                               ZipCode                         { get; set; }
        int?                                 CityAddressId                   { get; set; }
        ICollection<IParticipantContactInfo> ParticipantContactInfoes        { get; set; }
        ICity                                City                            { get; set; }
        IState                               State                           { get; set; }
        string                               ModifiedBy                      { get; set; }
        DateTime?                            ModifiedDate                    { get; set; }
        string                               AddressLine1                    { get; set; }
        string                               AddressLine2                    { get; set; }
        int?                                 AddressVerificationTypeLookupId { get; set; }
        IAddressVerificationTypeLookup       AddressVerificationTypeLookup   { get; set; }
    }
}
