using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class NonCustodialChild
    {
        #region Properties

        public int       NonCustodialCaretakerId              { get; set; }
        public string    FirstName                            { get; set; }
        public string    LastName                             { get; set; }
        public DateTime? DateOfBirth                          { get; set; }
        public bool?     HasChildSupportOrder                 { get; set; }
        public string    ChildSupportOrderDetails             { get; set; }
        public int?      ContactIntervalId                    { get; set; }
        public string    ContactIntervalDetails               { get; set; }
        public string    OtherAdultsDetails                   { get; set; }
        public bool?     IsRelationshipChangeRequested        { get; set; }
        public string    RelationshipChangeRequestedDetails   { get; set; }
        public string    NeedOfServicesDetails                { get; set; }
        public string    ModifiedBy                           { get; set; }
        public DateTime? ModifiedDate                         { get; set; }
        public int?      DeleteReasonId                       { get; set; }
        public int?      HasOtherAdultsYesNoUnknownLookupId   { get; set; }
        public int?      IsNeedOfServicesYesNoUnknownLookupId { get; set; }
        public bool?     HasNameOnChildBirthRecord            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ContactInterval       ContactInterval                    { get; set; }
        public virtual NonCustodialCaretaker NonCustodialCaretaker              { get; set; }
        public virtual DeleteReason          DeleteReason                       { get; set; }
        public virtual YesNoUnknownLookup    HasOtherAdultsYesNoUnknownLookup   { get; set; }
        public virtual YesNoUnknownLookup    IsNeedOfServicesYesNoUnknownLookup { get; set; }

        #endregion
    }
}
