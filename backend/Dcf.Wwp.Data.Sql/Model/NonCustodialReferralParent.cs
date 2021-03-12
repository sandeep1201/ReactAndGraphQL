using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonCustodialReferralParent
    {
        #region Properties

        public int       NonCustodialReferralParentsSectionId { get; set; }
        public string    FirstName                            { get; set; }
        public string    LastName                             { get; set; }
        public bool?     IsAvailableOrWorking                 { get; set; }
        public string    AvailableOrWorkingDetails            { get; set; }
        public bool?     IsInterestedInWorkProgram            { get; set; }
        public string    InterestedInWorkProgramDetails       { get; set; }
        public bool?     IsContactKnownWithParent             { get; set; }
        public int?      ContactId                            { get; set; }
        public int?      DeleteReasonId                       { get; set; }
        public string    ModifiedBy                           { get; set; }
        public DateTime? ModifiedDate                         { get; set; }

        #endregion

        #region Navigation Properties

        public virtual NonCustodialParentsReferralSection     NonCustodialReferralParentsSection { get; set; }
        public virtual Contact                                Contact                            { get; set; }
        public virtual DeleteReason                           DeleteReason                       { get; set; }
        public virtual ICollection<NonCustodialReferralChild> NonCustodialReferralChilds         { get; set; }

        #endregion
    }
}
