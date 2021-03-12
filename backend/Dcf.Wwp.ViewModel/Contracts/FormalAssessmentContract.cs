using System.Diagnostics;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class FormalAssessmentContract : BaseContract, IIsEmpty
    {
        public string ReferralDate                               { get; set; }
        public bool?  ReferralDeclined                           { get; set; }
        public string ReferralDetails                            { get; set; }
        public string AssessmentDate                             { get; set; }
        public bool?  AssessmentNotCompleted                     { get; set; }
        public string AssessmentDetails                          { get; set; }
        public int?   SymptomId                                  { get; set; }
        public bool?  IsRecommendedDateNotNeeded                 { get; set; }
        public string SymptomName                                { get; set; }
        public string ReassessmentRecommendedDate                { get; set; }
        public string SymptomDetails                             { get; set; }
        public int?   HoursParticipantCanParticipate             { get; set; }
        public string HoursParticipantCanParticipateDetails      { get; set; }
        public int?   HoursParticipantCanParticipateIntervalId   { get; set; }
        public string HoursParticipantCanParticipateIntervalDesc { get; set; }
        public int?   AssessmentProviderContactId                { get; set; }
        public int?   DeleteReasonId                             { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(ReferralDate)                          &&
                   ReferralDeclined == null                                         &&
                   string.IsNullOrWhiteSpace(ReferralDetails)                       &&
                   string.IsNullOrWhiteSpace(AssessmentDate)                        &&
                   AssessmentNotCompleted == null                                   &&
                   string.IsNullOrWhiteSpace(AssessmentDetails)                     &&
                   SymptomId == null                                                &&
                   string.IsNullOrWhiteSpace(ReassessmentRecommendedDate)           &&
                   IsRecommendedDateNotNeeded == null                               &&
                   string.IsNullOrWhiteSpace(SymptomDetails)                        &&
                   HoursParticipantCanParticipate == null                           &&
                   string.IsNullOrWhiteSpace(HoursParticipantCanParticipateDetails) &&
                   HoursParticipantCanParticipateIntervalId == null                 &&
                   AssessmentProviderContactId              == null;
        }

        #endregion IIsEmpty

        public static bool AdoptIfSimilarToModel<TContract, TModel>(TContract contract, TModel model)
            where TContract : FormalAssessmentContract
            where TModel : IFormalAssessment
        {
            Debug.Assert(contract.IsNew(), "FormalAssessmentContract is expected to be new, but it's not.");

            // The logic for determining if a newly passed in contract matches values

            if (model != null)
            {
                if (contract.ReferralDate   != model.ReferralDate.ToString()               ||
                    contract.AssessmentDate != model.AssessmentDate.ToStringMonthDayYear() ||
                    //contract.Details != model.Details ||      // We will allow different details but it still be considered the same person.
                    contract.ReferralDetails             != model.ReferralDetails                                    ||
                    contract.ReferralDeclined            != model.ReferralDeclined                                   ||
                    contract.ReassessmentRecommendedDate != model.ReassessmentRecommendedDate.ToStringMonthDayYear() ||
                    contract.SymptomId                   != model.SymptomId                                          ||
                    contract.AssessmentProviderContactId != model.AssessmentProviderContactId                        ||
                    contract.SymptomDetails              != model.SymptomDetails                                     ||
                    contract.AssessmentNotCompleted      != model.AssessmentNotCompleted                             ||
                    contract.AssessmentDetails           != model.AssessmentDetails
                    )
                    return false;

                // If we get here, we have a match.  Since it is, we need to adopt the model's
                // Id values.
                contract.Id = model.Id;
                return true;
            }

            return true;
        }
    }
}
