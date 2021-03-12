using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FamilyBarriersSection
    {
        #region Properties

        public int       ParticipantId                                    { get; set; }
        public bool?     HasEverAppliedSsi                                { get; set; }
        public bool?     IsCurrentlyApplyingSsi                           { get; set; }
        public int?      SsiApplicationStatusId                           { get; set; }
        public int?      SsiApplicationStatusDetailsId                    { get; set; }
        public DateTime? SsiApplicationDate                               { get; set; }
        public bool?     SsiApplicationIsAnyoneHelping                    { get; set; }
        public int?      SsiApplicationDetailsId                          { get; set; }
        public int?      SsiApplicationContactId                          { get; set; }
        public bool?     HasReceivedPastSsi                               { get; set; }
        public int?      PastSsiDetailsId                                 { get; set; }
        public bool?     HasDeniedSsi                                     { get; set; }
        public DateTime? DeniedSsiDate                                    { get; set; }
        public int?      DeniedSsiDetailsId                               { get; set; }
        public bool?     IsInterestedInLearningMoreSsi                    { get; set; }
        public int?      InterestedInLearningMoreSsiDetailsId             { get; set; }
        public bool?     HasAnyoneAppliedForSsi                           { get; set; }
        public bool?     IsAnyoneReceivingSsi                             { get; set; }
        public int?      AnyoneReceivingSsiDetailsId                      { get; set; }
        public bool?     IsAnyoneApplyingForSsi                           { get; set; }
        public int?      AnyoneApplyingForSsiDetailsId                    { get; set; }
        public bool?     HasCaretakingResponsibilities                    { get; set; }
        public bool?     HasConcernsAboutCaretakingResponsibilities       { get; set; }
        public int?      ConcernsAboutCaretakingResponsibilitiesDetailsId { get; set; }
        public bool?     DoesHouseholdEngageInRiskyActivities             { get; set; }
        public int?      HouseholdEngageInRiskyActivitiesDetailsId        { get; set; }
        public bool?     DoChildrenHaveBehaviourProblems                  { get; set; }
        public int?      ChildrenHaveBehaviourProblemsDetailsId           { get; set; }
        public bool?     AreChildrenAtRiskOfSchoolSuspension              { get; set; }
        public int?      ChildrenAtRiskOfSchoolSuspensionDetailsId        { get; set; }
        public bool?     AreAnyFamilyIssuesAffectWork                     { get; set; }
        public int?      AnyFamilyIssuesAffectWorkDetailsId               { get; set; }
        public int?      ReasonForPastSsiDetailsId                        { get; set; }
        public string    Notes                                            { get; set; }
        public bool      IsDeleted                                        { get; set; }
        public string    ModifiedBy                                       { get; set; }
        public DateTime? ModifiedDate                                     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ApplicationStatusType     ApplicationStatusType                                       { get; set; }
        public virtual Contact                   Contact                                                     { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersPastSsiDetail                                 { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersAnyFamilyIssuesAffectWorkDetail               { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersAnyoneApplyingForSsiDetail                    { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersAnyOneReceivingSsiDetail                      { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersChildrenHaveBehaviourProblemsDetail           { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersHouseholdEngageRiskyActivitiesDetail          { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersInterestedInLearningMoreSsiDetail             { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersSsiApplicationDetail                          { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersSsiApplicationStatusDetail                    { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail        { get; set; }
        public virtual FamilyBarriersDetail      FamilyBarriersDeniedSsiDetail                               { get; set; }
        public virtual Participant               Participant                                                 { get; set; }
        public virtual ICollection<FamilyMember> FamilyMembers                                               { get; set; } = new List<FamilyMember>();
        public virtual FamilyBarriersDetail      FamilyBarriersReasonForPastSsiDetail                        { get; set; }

        #endregion
    }
}
