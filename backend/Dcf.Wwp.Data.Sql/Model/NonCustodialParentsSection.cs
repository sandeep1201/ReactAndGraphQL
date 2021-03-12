using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonCustodialParentsSection
    {
        #region Properties

        public int       ParticipantId                       { get; set; }
        public bool?     HasChildren                         { get; set; }
        public decimal?  ChildSupportPayment                 { get; set; }
        public bool?     HasOwedChildSupport                 { get; set; }
        public bool?     HasInterestInChildServices          { get; set; }
        public bool?     IsInterestedInReferralServices      { get; set; }
        public string    InterestedInReferralServicesDetails { get; set; }
        public string    Notes                               { get; set; }
        public string    ModifiedBy                          { get; set; }
        public DateTime? ModifiedDate                        { get; set; }
        public int?      ChildSupportContactId               { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                        Participant            { get; set; }
        public virtual Contact                            ChildSupportContact    { get; set; }
        public virtual ICollection<NonCustodialCaretaker> NonCustodialCaretakers { get; set; } = new List<NonCustodialCaretaker>();

        #endregion
    }
}
