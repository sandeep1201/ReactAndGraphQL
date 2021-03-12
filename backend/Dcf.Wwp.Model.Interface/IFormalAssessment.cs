using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IFormalAssessment : ICommonModel, IHasDeleteReason, ICloneable
    {
        int?      BarrierDetailsId                         { get; set; }
        DateTime? ReferralDate                             { get; set; }
        bool?     ReferralDeclined                         { get; set; }
        string    ReferralDetails                          { get; set; }
        DateTime? AssessmentDate                           { get; set; }
        bool?     IsRecommendedDateNotNeeded               { get; set; }
        bool?     AssessmentNotCompleted                   { get; set; }
        string    AssessmentDetails                        { get; set; }
        int?      SymptomId                                { get; set; }
        DateTime? ReassessmentRecommendedDate              { get; set; }
        string    SymptomDetails                           { get; set; }
        int?      AssessmentProviderContactId              { get; set; }
        int?      HoursParticipantCanParticipate           { get; set; }
        string    HoursParticipantCanParticipateDetails    { get; set; }
        int?      HoursParticipantCanParticipateIntervalId { get; set; }

        IBarrierDetail BarrierDetail { get; set; }
        ISymptom       Symptom       { get; set; }
        IIntervalType  IntervalType  { get; set; }
    }
}
