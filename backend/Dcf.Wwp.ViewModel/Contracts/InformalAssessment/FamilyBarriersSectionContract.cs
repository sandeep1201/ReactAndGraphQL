using FluentValidation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;
using Dcf.Wwp.Api.Library.Contracts.Cww;
using Dcf.Wwp.Api.Library.Validators;
using Dcf.Wwp.Api.Library.Validators.ValidatorExtensions;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class FamilyBarriersSectionContract : BaseInformalAssessmentContract, IValidatableObject
    {
        private readonly IValidator _validator;

        public bool? HasEverAppliedSsi { get; set; }

        public bool? IsCurrentlyApplyingSsi { get; set; }

        public int? SsiApplicationStatusId { get; set; }

        public string SsiApplicationStatusName { get; set; }

        public string SsiApplicationStatusDetails { get; set; }

        public string SsiApplicationDate { get; set; }

        public bool? SsiApplicationIsAnyoneHelping { get; set; }

        public string SsiApplicationDetails { get; set; }

        public int? SsiApplicationContactId { get; set; }

        public bool? HasReceivedPastSsi { get; set; }

        public string PastSsiDetails { get; set; }

        public bool? HasDeniedSsi { get; set; }

        public string DeniedSsiDate { get; set; }

        public string DeniedSsiDetails { get; set; }

        public bool? IsInterestedInLearningMoreSsi { get; set; }

        public string InterestedInLearningMoreSsiDetails { get; set; }

        public bool? HasAnyoneAppliedForSsi { get; set; }

        public bool? IsAnyoneReceivingSsi { get; set; }

        public string AnyoneReceivingSsiDetails { get; set; }

        public bool? IsAnyoneApplyingForSsi { get; set; }

        public string AnyoneApplyingForSsiDetails { get; set; }

        public bool? HasCaretakingResponsibilities { get; set; }

        public bool? HasConcernsAboutCaretakingResponsibilities { get; set; }

        public string ConcernsAboutCaretakingResponsibilitiesDetails { get; set; }

        public bool? DoesHouseholdEngageInRiskyActivities { get; set; }

        public string HouseholdEngageInRiskyActivitiesDetails { get; set; }

        public bool? DoChildrenHaveBehaviourProblems { get; set; }

        public string ChildrenHaveBehaviourProblemsDetails { get; set; }

        public bool? AreChildrenAtRiskOfSchoolSuspension { get; set; }

        public string ChildrenAtRiskOfSchoolSuspensionDetails { get; set; }

        public bool? AreAnyFamilyIssuesAffectWork { get; set; }

        public string AnyFamilyIssuesAffectWorkDetails { get; set; }
        public string FamilyBarriersReasonForPastSsiDetails { get; set; }

        public List<FamilyMemberContract> FamilyMembers { get; set; }

        public List<FamilyMemberContract> DeletedFamilyMembers { get; set; }

        public ActionNeededContract ActionNeeded { get; set; }

        public string Notes { get; set; }
        // commenting out this to hide the referential data on the FamilyVarriers based on a Change Request
        public List<Learnfare> CwwLearnfare { get; set; }

        //public List<SocialSecurityStatus> CwwSocialSecurityStatus { get; set; }

        #region Constructor

        public FamilyBarriersSectionContract()
        {
            _validator = new FamilyBarriersSectionValidator();
        }

        #endregion

        #region IValidatableObject
        ///
        /// Determines whether the specified object is valid.
        ///
        ///The validation context.
        ///
        /// A collection that holds failed-validation information.
        ///
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = _validator.Validate(this).ToValidationResult();
            return results;
        }
        #endregion
    }
}
