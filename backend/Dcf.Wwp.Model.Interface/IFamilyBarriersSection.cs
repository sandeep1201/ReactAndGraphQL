using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IFamilyBarriersSection : ICommonDelModel, ICloneable
    {
        Int32 ParticipantId { get; set; }
        Boolean? HasEverAppliedSsi { get; set; }
        Boolean? IsCurrentlyApplyingSsi { get; set; }
        Int32? SsiApplicationStatusId { get; set; }
        Int32? SsiApplicationStatusDetailsId { get; set; }
        DateTime? SsiApplicationDate { get; set; }
        Boolean? SsiApplicationIsAnyoneHelping { get; set; }
        Int32? SsiApplicationDetailsId { get; set; }
        Int32? SsiApplicationContactId { get; set; }
        Boolean? HasReceivedPastSsi { get; set; }
        Int32? PastSsiDetailsId { get; set; }
        Boolean? HasDeniedSsi { get; set; }
        DateTime? DeniedSsiDate { get; set; }
        Int32? DeniedSsiDetailsId { get; set; }
        Boolean? IsInterestedInLearningMoreSsi { get; set; }
        Int32? InterestedInLearningMoreSsiDetailsId { get; set; }
        Boolean? HasAnyoneAppliedForSsi { get; set; }
        Boolean? IsAnyoneReceivingSsi { get; set; }
        Int32? AnyoneReceivingSsiDetailsId { get; set; }
        Boolean? IsAnyoneApplyingForSsi { get; set; }
        Int32? AnyoneApplyingForSsiDetailsId { get; set; }
        Boolean? HasCaretakingResponsibilities { get; set; }
        Boolean? HasConcernsAboutCaretakingResponsibilities { get; set; }
        Int32? ConcernsAboutCaretakingResponsibilitiesDetailsId { get; set; }
        Boolean? DoesHouseholdEngageInRiskyActivities { get; set; }
        Int32? HouseholdEngageInRiskyActivitiesDetailsId { get; set; }
        Boolean? DoChildrenHaveBehaviourProblems { get; set; }
        Int32? ChildrenHaveBehaviourProblemsDetailsId { get; set; }
        Boolean? AreChildrenAtRiskOfSchoolSuspension { get; set; }
        Int32? ChildrenAtRiskOfSchoolSuspensionDetailsId { get; set; }
        Boolean? AreAnyFamilyIssuesAffectWork { get; set; }
        Int32? AnyFamilyIssuesAffectWorkDetailsId { get; set; }
        String Notes { get; set; }
        int? ReasonForPastSsiDetailsId { get; set; }
        IFamilyBarriersDetail FamilyBarriersPastSsiDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersSsiApplicationStatusDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersSsiApplicationDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersDeniedSsiDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersAnyoneApplyingForSsiDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersHouseholdEngageRiskyActivitiesDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersChildrenHaveBehaviourProblemsDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersAnyFamilyIssuesAffectWorkDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersAnyOneReceivingSsiDetail { get; set; }
        IFamilyBarriersDetail FamilyBarriersInterestedInLearningMoreSsiDetail { get; set; }
        IParticipant Participant { get; set; }
        ICollection<IFamilyMember> FamilyMembers { get; set; }
        /// <summary>
        /// PreTeens contains all the (non-deleted) FamilyMembers.
        /// </summary>
        ICollection<IFamilyMember> NonDeletedFamilyMembers { get; }
        IApplicationStatusType ApplicationStatusType { get; set; }
        IContact Contact { get; set; }
        IFamilyBarriersDetail FamilyBarriersReasonForPastSsiDetail { get; set; }

    }
}
