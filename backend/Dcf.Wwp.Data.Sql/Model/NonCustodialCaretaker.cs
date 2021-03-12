using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonCustodialCaretaker
    {
        #region Properties

        public int       NonCustodialParentsSectionId            { get; set; }
        public string    FirstName                               { get; set; }
        public bool      IsFirstNameUnknown                      { get; set; }
        public string    LastName                                { get; set; }
        public bool      IsLastNameUnknown                       { get; set; }
        public int?      NonCustodialParentRelationshipId        { get; set; }
        public string    RelationshipDetails                     { get; set; }
        public int?      ContactIntervalId                       { get; set; }
        public string    ContactIntervalDetails                  { get; set; }
        public bool?     IsRelationshipChangeRequested           { get; set; }
        public string    RelationshipChangeRequestedDetails      { get; set; }
        public bool?     IsInterestedInRelationshipReferral      { get; set; }
        public string    InterestedInRelationshipReferralDetails { get; set; }
        public int?      DeleteReasonId                          { get; set; }
        public string    ModifiedBy                              { get; set; }
        public DateTime? ModifiedDate                            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual NonCustodialParentsSection     NonCustodialParentsSection     { get; set; }
        public virtual NonCustodialParentRelationship NonCustodialParentRelationship { get; set; }
        public virtual ContactInterval                ContactInterval                { get; set; }
        public virtual DeleteReason                   DeleteReason                   { get; set; }
        public virtual ICollection<NonCustodialChild> NonCustodialChilds             { get; set; }

        #endregion
    }
}
