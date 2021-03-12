using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AddressVerificationTypeLookup
    {
        #region Properties

        public int      Id           { get; set; }
        public string   Name         { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte[]   RowVersion   { get; set; }

        public virtual ICollection<AlternateMailingAddress> AlternateMailingAddresses { get; set; }
        public virtual ICollection<ParticipantContactInfo>  ParticipantContactInfoes  { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
