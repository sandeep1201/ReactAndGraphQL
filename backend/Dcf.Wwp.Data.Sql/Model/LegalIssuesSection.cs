using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class LegalIssuesSection
    {
        #region Properties

        public int       ParticipantId                  { get; set; }
        public bool?     IsConvictedOfCrime             { get; set; }
        public bool?     IsUnderCommunitySupervision    { get; set; }
        public string    CommunitySupervisonDetails     { get; set; }
        public bool?     HasPendingCharges              { get; set; }
        public bool?     HasFamilyLegalIssues           { get; set; }
        public string    FamilyLegalIssueNotes          { get; set; }
        public bool?     HasCourtDates                  { get; set; }
        public string    ActionNeededDetails            { get; set; }
        public bool?     OrderedToPayChildSupport       { get; set; }
        public decimal?  MonthlyAmount                  { get; set; }
        public bool?     IsUnknown                      { get; set; }
        public bool?     OweAnyChildSupportBack         { get; set; }
        public string    ChildSupportDetails            { get; set; }
        public int?      CommunitySupervisonContactId   { get; set; }
        public string    Notes                          { get; set; }
        public bool?     HasRestrainingOrders           { get; set; }
        public string    RestrainingOrderNotes          { get; set; }
        public bool?     HasRestrainingOrderToPrevent   { get; set; }
        public string    RestrainingOrderToPreventNotes { get; set; }
        public bool      IsDeleted                      { get; set; }
        public string    ModifiedBy                     { get; set; }
        public DateTime? ModifiedDate                   { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                Participant    { get; set; }
        public virtual Contact                    Contact        { get; set; }
        public virtual ICollection<Conviction>    Convictions    { get; set; } = new List<Conviction>();
        public virtual ICollection<CourtDate>     CourtDates     { get; set; } = new List<CourtDate>();
        public virtual ICollection<PendingCharge> PendingCharges { get; set; } = new List<PendingCharge>();

        #endregion
    }
}
