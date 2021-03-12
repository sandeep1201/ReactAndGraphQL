using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AlternateMailingAddress
    {
        #region Properties

        public string    ZipCode                         { get; set; }
        public int?      CityAddressId                   { get; set; }
        public int?      StateId                         { get; set; }
        public bool      IsDeleted                       { get; set; }
        public string    ModifiedBy                      { get; set; }
        public DateTime? ModifiedDate                    { get; set; }
        public string    StreetAddressPlaceId            { get; set; }
        public string    AddressLine1                    { get; set; }
        public string    AddressLine2                    { get; set; }
        public int?      AddressVerificationTypeLookupId { get; set; }

        #endregion

        #region Navigation properties

        public virtual City                                City                          { get; set; }
        public virtual State                               State                         { get; set; }
        public virtual ICollection<ParticipantContactInfo> ParticipantContactInfoes      { get; set; }
        public virtual AddressVerificationTypeLookup       AddressVerificationTypeLookup { get; set; }

        #endregion
    }
}
