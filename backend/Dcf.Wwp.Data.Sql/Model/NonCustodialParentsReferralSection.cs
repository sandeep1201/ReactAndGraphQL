using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonCustodialParentsReferralSection
    {
        #region Properties

        public int       ParticipantId { get; set; }
        public int?      HasChildrenId { get; set; }
        public string    Notes         { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime? ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                             Participant                 { get; set; }
        public virtual YesNoSkipLookup                         YesNoSkipLookup             { get; set; }
        public virtual ICollection<NonCustodialReferralParent> NonCustodialReferralParents { get; set; } = new List<NonCustodialReferralParent>();

        #endregion
    }
}
