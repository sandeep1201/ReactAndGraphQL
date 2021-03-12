using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FormalAssessment : BaseEntity, IFormalAssessment, IEquatable<FormalAssessment>
    {
        IBarrierDetail IFormalAssessment.BarrierDetail
        {
            get { return BarrierDetail; }
            set { BarrierDetail = (BarrierDetail) value; }
        }

        ISymptom IFormalAssessment.Symptom
        {
            get { return Symptom; }
            set { Symptom = (Symptom) value; }
        }

        IDeleteReason IHasDeleteReason.DeleteReason
        {
            get { return DeleteReason; }

            set { DeleteReason = (DeleteReason) value; }
        }

        IIntervalType IFormalAssessment.IntervalType
        {
            get { return IntervalType; }
            set { IntervalType = (IntervalType) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var fa = new FormalAssessment
                     {
                         Id                                       = Id,
                         ReferralDate                             = ReferralDate,
                         ReferralDeclined                         = ReferralDeclined,
                         ReferralDetails                          = ReferralDetails,
                         AssessmentDate                           = AssessmentDate,
                         AssessmentNotCompleted                   = AssessmentNotCompleted,
                         AssessmentDetails                        = AssessmentDetails,
                         SymptomId                                = SymptomId,
                         ReassessmentRecommendedDate              = ReassessmentRecommendedDate,
                         SymptomDetails                           = SymptomDetails,
                         HoursParticipantCanParticipate           = HoursParticipantCanParticipate,
                         HoursParticipantCanParticipateDetails    = HoursParticipantCanParticipateDetails,
                         HoursParticipantCanParticipateIntervalId = HoursParticipantCanParticipateIntervalId,
                         AssessmentProviderContactId              = AssessmentProviderContactId,
                         DeleteReasonId                           = DeleteReasonId
                     };

            return fa;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as FormalAssessment;
            return obj != null && Equals(obj);
        }

        public bool Equals(FormalAssessment other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ReferralDate, other.ReferralDate))
                return false;
            if (!AdvEqual(ReferralDeclined, other.ReferralDeclined))
                return false;
            if (!AdvEqual(ReferralDetails, other.ReferralDetails))
                return false;
            if (!AdvEqual(AssessmentDate, other.AssessmentDate))
                return false;
            if (!AdvEqual(AssessmentNotCompleted, other.AssessmentNotCompleted))
                return false;
            if (!AdvEqual(AssessmentDetails, other.AssessmentDetails))
                return false;
            if (!AdvEqual(SymptomId, other.SymptomId))
                return false;
            if (!AdvEqual(SymptomDetails, other.SymptomDetails))
                return false;
            if (!AdvEqual(ReassessmentRecommendedDate, other.ReassessmentRecommendedDate))
                return false;
            if (!AdvEqual(IsRecommendedDateNotNeeded, other.IsRecommendedDateNotNeeded))
                return false;
            if (!AdvEqual(HoursParticipantCanParticipate, other.HoursParticipantCanParticipate))
                return false;
            if (!AdvEqual(HoursParticipantCanParticipateDetails, other.HoursParticipantCanParticipateDetails))
                return false;
            if (!AdvEqual(HoursParticipantCanParticipateIntervalId, other.HoursParticipantCanParticipateIntervalId))
                return false;
            if (!AdvEqual(AssessmentProviderContactId, other.AssessmentProviderContactId))
                return false;
            if (!AdvEqual(DeleteReasonId, other.DeleteReasonId))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
