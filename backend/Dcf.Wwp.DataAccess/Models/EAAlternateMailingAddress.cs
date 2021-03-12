using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAAlternateMailingAddress : BaseEntity
    {
        #region Properties

        public string   AddressLine1                    { get; set; }
        public string   AddressLine2                    { get; set; }
        public string   ZipCode                         { get; set; }
        public int      CityAddressId                   { get; set; }
        public int      AddressVerificationTypeLookupId { get; set; }
        public bool     IsDeleted                       { get; set; }
        public string   ModifiedBy                      { get; set; }
        public DateTime ModifiedDate                    { get; set; }

        #endregion

        #region Navigation properties

        public virtual City                              City                          { get; set; }
        public virtual ICollection<EARequestContactInfo> EARequestContactInfoes        { get; set; }
        public virtual AddressVerificationTypeLookup     AddressVerificationTypeLookup { get; set; }
        public virtual ICollection<EAPayment>            EaPayments                    { get; set; }
        public virtual ICollection<EAIPV>                EaIpvs                        { get; set; }

        #endregion
    }
}
