using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FormalAssessment
    {
        #region Properties

        public int?      BarrierDetailsId                         { get; set; }
        public DateTime? ReferralDate                             { get; set; }
        public bool?     ReferralDeclined                         { get; set; }
        public string    ReferralDetails                          { get; set; }
        public DateTime? AssessmentDate                           { get; set; }
        public bool?     AssessmentNotCompleted                   { get; set; }
        public string    AssessmentDetails                        { get; set; }
        public int?      SymptomId                                { get; set; }
        public DateTime? ReassessmentRecommendedDate              { get; set; }
        public bool?     IsRecommendedDateNotNeeded               { get; set; }
        public string    SymptomDetails                           { get; set; }
        public int?      AssessmentProviderContactId              { get; set; }
        public int?      HoursParticipantCanParticipate           { get; set; }
        public string    HoursParticipantCanParticipateDetails    { get; set; }
        public int?      HoursParticipantCanParticipateIntervalId { get; set; }
        public int?      DeleteReasonId                           { get; set; }
        public DateTime? CreatedDate                              { get; set; }
        public string    ModifiedBy                               { get; set; }
        public DateTime? ModifiedDate                             { get; set; }

        #endregion

        #region Navigation Properties

        public virtual BarrierDetail BarrierDetail { get; set; }
        public virtual Symptom       Symptom       { get; set; }
        public virtual IntervalType  IntervalType  { get; set; }
        public virtual DeleteReason  DeleteReason  { get; set; }

        #endregion
    }
}
