using Dcf.Wwp.Model.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class FamilyBarriersSection : BaseCommonModel, IFamilyBarriersSection, IComplexModel
    {
        IParticipant IFamilyBarriersSection.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        IApplicationStatusType IFamilyBarriersSection.ApplicationStatusType
        {
            get { return ApplicationStatusType; }
            set { ApplicationStatusType = (ApplicationStatusType) value; }
        }

        IContact IFamilyBarriersSection.Contact
        {
            get { return Contact; }
            set { Contact = (Contact) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersPastSsiDetail
        {
            get { return FamilyBarriersPastSsiDetail; }
            set { FamilyBarriersPastSsiDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersSsiApplicationStatusDetail
        {
            get { return FamilyBarriersSsiApplicationStatusDetail; }
            set { FamilyBarriersSsiApplicationStatusDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersSsiApplicationDetail
        {
            get { return FamilyBarriersSsiApplicationDetail; }
            set { FamilyBarriersSsiApplicationDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersDeniedSsiDetail
        {
            get { return FamilyBarriersDeniedSsiDetail; }
            set { FamilyBarriersDeniedSsiDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersAnyoneApplyingForSsiDetail
        {
            get { return FamilyBarriersAnyoneApplyingForSsiDetail; }
            set { FamilyBarriersAnyoneApplyingForSsiDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail
        {
            get { return FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail; }
            set { FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersHouseholdEngageRiskyActivitiesDetail
        {
            get { return FamilyBarriersHouseholdEngageRiskyActivitiesDetail; }
            set { FamilyBarriersHouseholdEngageRiskyActivitiesDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersChildrenHaveBehaviourProblemsDetail
        {
            get { return FamilyBarriersChildrenHaveBehaviourProblemsDetail; }
            set { FamilyBarriersChildrenHaveBehaviourProblemsDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail
        {
            get { return FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail; }
            set { FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersAnyFamilyIssuesAffectWorkDetail
        {
            get { return FamilyBarriersAnyFamilyIssuesAffectWorkDetail; }
            set { FamilyBarriersAnyFamilyIssuesAffectWorkDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersAnyOneReceivingSsiDetail
        {
            get { return FamilyBarriersAnyOneReceivingSsiDetail; }
            set { FamilyBarriersAnyOneReceivingSsiDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersInterestedInLearningMoreSsiDetail
        {
            get { return FamilyBarriersInterestedInLearningMoreSsiDetail; }
            set { FamilyBarriersInterestedInLearningMoreSsiDetail = (FamilyBarriersDetail) value; }
        }

        IFamilyBarriersDetail IFamilyBarriersSection.FamilyBarriersReasonForPastSsiDetail
        {
            get { return FamilyBarriersReasonForPastSsiDetail; }
            set { FamilyBarriersReasonForPastSsiDetail = (FamilyBarriersDetail) value; }
        }

        ICollection<IFamilyMember> IFamilyBarriersSection.FamilyMembers
        {
            get { return FamilyMembers.Cast<IFamilyMember>().ToList(); }
            set { FamilyMembers = value.Cast<FamilyMember>().ToList(); }
        }

        ICollection<IFamilyMember> IFamilyBarriersSection.NonDeletedFamilyMembers => (from x in FamilyMembers where x.DeleteReasonId == null select x).Cast<IFamilyMember>().ToList();

        #region ICloneable

        public object Clone()
        {
            var clone = new FamilyBarriersSection
                        {
                            HasEverAppliedSsi                                           = this.HasEverAppliedSsi,
                            IsCurrentlyApplyingSsi                                      = this.IsCurrentlyApplyingSsi,
                            SsiApplicationStatusId                                      = this.SsiApplicationStatusId,
                            SsiApplicationStatusDetailsId                               = this.SsiApplicationStatusDetailsId,
                            FamilyBarriersSsiApplicationStatusDetail                    = (FamilyBarriersDetail) this.FamilyBarriersSsiApplicationStatusDetail?.Clone(),
                            SsiApplicationDate                                          = this.SsiApplicationDate,
                            SsiApplicationIsAnyoneHelping                               = this.SsiApplicationIsAnyoneHelping,
                            SsiApplicationDetailsId                                     = this.SsiApplicationDetailsId,
                            FamilyBarriersSsiApplicationDetail                          = (FamilyBarriersDetail) this.FamilyBarriersSsiApplicationDetail?.Clone(),
                            SsiApplicationContactId                                     = this.SsiApplicationContactId,
                            HasReceivedPastSsi                                          = this.HasReceivedPastSsi,
                            PastSsiDetailsId                                            = this.PastSsiDetailsId,
                            FamilyBarriersPastSsiDetail                                 = (FamilyBarriersDetail) this.FamilyBarriersPastSsiDetail?.Clone(),
                            HasDeniedSsi                                                = this.HasDeniedSsi,
                            DeniedSsiDate                                               = this.DeniedSsiDate,
                            DeniedSsiDetailsId                                          = this.DeniedSsiDetailsId,
                            FamilyBarriersDeniedSsiDetail                               = (FamilyBarriersDetail) this.FamilyBarriersDeniedSsiDetail?.Clone(),
                            IsInterestedInLearningMoreSsi                               = this.IsInterestedInLearningMoreSsi,
                            InterestedInLearningMoreSsiDetailsId                        = this.InterestedInLearningMoreSsiDetailsId,
                            FamilyBarriersInterestedInLearningMoreSsiDetail             = (FamilyBarriersDetail) this.FamilyBarriersInterestedInLearningMoreSsiDetail?.Clone(),
                            HasAnyoneAppliedForSsi                                      = this.HasAnyoneAppliedForSsi,
                            IsAnyoneReceivingSsi                                        = this.IsAnyoneReceivingSsi,
                            AnyoneReceivingSsiDetailsId                                 = this.AnyoneReceivingSsiDetailsId,
                            FamilyBarriersAnyOneReceivingSsiDetail                      = (FamilyBarriersDetail) this.FamilyBarriersAnyOneReceivingSsiDetail?.Clone(),
                            IsAnyoneApplyingForSsi                                      = this.IsAnyoneApplyingForSsi,
                            AnyoneApplyingForSsiDetailsId                               = this.AnyoneApplyingForSsiDetailsId,
                            FamilyBarriersAnyoneApplyingForSsiDetail                    = (FamilyBarriersDetail) this.FamilyBarriersAnyoneApplyingForSsiDetail?.Clone(),
                            HasCaretakingResponsibilities                               = this.HasCaretakingResponsibilities,
                            HasConcernsAboutCaretakingResponsibilities                  = this.HasConcernsAboutCaretakingResponsibilities,
                            ConcernsAboutCaretakingResponsibilitiesDetailsId            = this.ConcernsAboutCaretakingResponsibilitiesDetailsId,
                            FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail = (FamilyBarriersDetail) this.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail?.Clone(),
                            DoesHouseholdEngageInRiskyActivities                        = this.DoesHouseholdEngageInRiskyActivities,
                            HouseholdEngageInRiskyActivitiesDetailsId                   = this.HouseholdEngageInRiskyActivitiesDetailsId,
                            FamilyBarriersHouseholdEngageRiskyActivitiesDetail          = (FamilyBarriersDetail) this.FamilyBarriersHouseholdEngageRiskyActivitiesDetail?.Clone(),
                            DoChildrenHaveBehaviourProblems                             = this.DoChildrenHaveBehaviourProblems,
                            ChildrenHaveBehaviourProblemsDetailsId                      = this.ChildrenHaveBehaviourProblemsDetailsId,
                            FamilyBarriersChildrenHaveBehaviourProblemsDetail           = (FamilyBarriersDetail) this.FamilyBarriersChildrenHaveBehaviourProblemsDetail?.Clone(),
                            AreChildrenAtRiskOfSchoolSuspension                         = this.AreChildrenAtRiskOfSchoolSuspension,
                            ChildrenAtRiskOfSchoolSuspensionDetailsId                   = this.ChildrenAtRiskOfSchoolSuspensionDetailsId,
                            FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail        = (FamilyBarriersDetail) this.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail?.Clone(),
                            AreAnyFamilyIssuesAffectWork                                = this.AreAnyFamilyIssuesAffectWork,
                            AnyFamilyIssuesAffectWorkDetailsId                          = this.AnyFamilyIssuesAffectWorkDetailsId,
                            FamilyBarriersAnyFamilyIssuesAffectWorkDetail               = (FamilyBarriersDetail) this.FamilyBarriersAnyFamilyIssuesAffectWorkDetail?.Clone(),
                            ParticipantId                                               = this.ParticipantId,
                            Notes                                                       = this.Notes,
                            FamilyMembers                                               = this.FamilyMembers.Select(x => (FamilyMember) x.Clone()).ToList(),
                            ReasonForPastSsiDetailsId                                   = this.ReasonForPastSsiDetailsId,
                            FamilyBarriersReasonForPastSsiDetail                        = (FamilyBarriersDetail) this.FamilyBarriersReasonForPastSsiDetail?.Clone()
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as FamilyBarriersSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(FamilyBarriersSection other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            if (!AdvEqual(ParticipantId, other.ParticipantId))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(HasEverAppliedSsi, other.HasEverAppliedSsi))
                return false;
            if (!AdvEqual(IsCurrentlyApplyingSsi, other.IsCurrentlyApplyingSsi))
                return false;
            if (!AdvEqual(SsiApplicationStatusId, other.SsiApplicationStatusId))
                return false;

            if (!AdvEqual(SsiApplicationStatusDetailsId, other.SsiApplicationStatusDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersSsiApplicationStatusDetail, other.FamilyBarriersSsiApplicationStatusDetail))
                return false;

            if (!AdvEqual(SsiApplicationDate, other.SsiApplicationDate))
                return false;
            if (!AdvEqual(SsiApplicationIsAnyoneHelping, other.SsiApplicationIsAnyoneHelping))
                return false;
            if (!AdvEqual(SsiApplicationDetailsId, other.SsiApplicationDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersSsiApplicationDetail, other.FamilyBarriersSsiApplicationDetail))
                return false;

            if (!AdvEqual(SsiApplicationContactId, other.SsiApplicationContactId))
                return false;
            if (!AdvEqual(HasReceivedPastSsi, other.HasReceivedPastSsi))
                return false;

            if (!AdvEqual(PastSsiDetailsId, other.PastSsiDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersPastSsiDetail, other.FamilyBarriersPastSsiDetail))
                return false;

            if (!AdvEqual(HasDeniedSsi, other.HasDeniedSsi))
                return false;
            if (!AdvEqual(DeniedSsiDate, other.DeniedSsiDate))
                return false;

            if (!AdvEqual(DeniedSsiDetailsId, other.DeniedSsiDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersDeniedSsiDetail, other.FamilyBarriersDeniedSsiDetail))
                return false;

            if (!AdvEqual(IsInterestedInLearningMoreSsi, other.IsInterestedInLearningMoreSsi))
                return false;
            if (!AdvEqual(InterestedInLearningMoreSsiDetailsId, other.InterestedInLearningMoreSsiDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersInterestedInLearningMoreSsiDetail, other.FamilyBarriersInterestedInLearningMoreSsiDetail))
                return false;

            if (!AdvEqual(HasAnyoneAppliedForSsi, other.HasAnyoneAppliedForSsi))
                return false;
            if (!AdvEqual(IsAnyoneReceivingSsi, other.IsAnyoneReceivingSsi))
                return false;
            if (!AdvEqual(AnyoneReceivingSsiDetailsId, other.AnyoneReceivingSsiDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersAnyOneReceivingSsiDetail, other.FamilyBarriersAnyOneReceivingSsiDetail))
                return false;

            if (!AdvEqual(IsAnyoneApplyingForSsi, other.IsAnyoneApplyingForSsi))
                return false;
            if (!AdvEqual(AnyoneApplyingForSsiDetailsId, other.AnyoneApplyingForSsiDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersAnyoneApplyingForSsiDetail, other.FamilyBarriersAnyoneApplyingForSsiDetail))
                return false;

            if (!AdvEqual(HasCaretakingResponsibilities, other.HasCaretakingResponsibilities))
                return false;
            if (!AdvEqual(HasConcernsAboutCaretakingResponsibilities, other.HasConcernsAboutCaretakingResponsibilities))
                return false;
            if (!AdvEqual(ConcernsAboutCaretakingResponsibilitiesDetailsId, other.ConcernsAboutCaretakingResponsibilitiesDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail, other.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail))
                return false;

            if (!AdvEqual(DoesHouseholdEngageInRiskyActivities, other.DoesHouseholdEngageInRiskyActivities))
                return false;
            if (!AdvEqual(HouseholdEngageInRiskyActivitiesDetailsId, other.HouseholdEngageInRiskyActivitiesDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersHouseholdEngageRiskyActivitiesDetail, other.FamilyBarriersHouseholdEngageRiskyActivitiesDetail))
                return false;

            if (!AdvEqual(DoChildrenHaveBehaviourProblems, other.DoChildrenHaveBehaviourProblems))
                return false;
            if (!AdvEqual(ChildrenHaveBehaviourProblemsDetailsId, other.ChildrenHaveBehaviourProblemsDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersChildrenHaveBehaviourProblemsDetail, other.FamilyBarriersChildrenHaveBehaviourProblemsDetail))
                return false;

            if (!AdvEqual(AreChildrenAtRiskOfSchoolSuspension, other.AreChildrenAtRiskOfSchoolSuspension))
                return false;
            if (!AdvEqual(ChildrenAtRiskOfSchoolSuspensionDetailsId, other.ChildrenAtRiskOfSchoolSuspensionDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail, other.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail))
                return false;

            if (!AdvEqual(AreAnyFamilyIssuesAffectWork, other.AreAnyFamilyIssuesAffectWork))
                return false;
            if (!AdvEqual(AnyFamilyIssuesAffectWorkDetailsId, other.AnyFamilyIssuesAffectWorkDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersAnyFamilyIssuesAffectWorkDetail, other.FamilyBarriersAnyFamilyIssuesAffectWorkDetail))
                return false;

            if (AreBothNotNull(FamilyMembers, other.FamilyMembers) && !(FamilyMembers.OrderBy(x => x.Id).SequenceEqual(other.FamilyMembers.OrderBy(x => x.Id))))
                return false;

            if (!AdvEqual(ReasonForPastSsiDetailsId, other.ReasonForPastSsiDetailsId))
                return false;
            if (!AdvEqual(FamilyBarriersReasonForPastSsiDetail, other.FamilyBarriersReasonForPastSsiDetail))
                return false;

            return true;
        }

        #endregion IEquatable<T>

        #region IComplexModel

        public void SetModifiedOnComplexProperties<T>(T cloned, string user, DateTime modDate)
            where T : class, ICloneable, ICommonModel
        {
            // We don't need to set modified on null objects.
            if (cloned == null) return;

            Debug.Assert(cloned is IFamilyBarriersSection, "cloned is not IFamilyBarriersSection");

            var clone = (IFamilyBarriersSection) cloned;

            if (AreBothNotNull(FamilyMembers, clone.FamilyMembers))
            {
                var first  = FamilyMembers.OrderBy(x => x.Id).ToList();
                var second = clone.FamilyMembers.OrderBy(x => x.Id).ToList();

                int i = 0;
                foreach (var fms1 in first)
                {
                    // We only need to set the modified on existing objects.
                    if (fms1.Id != 0)
                    {
                        // Make sure there is a cloned object.
                        if (i < second.Count)
                        {
                            var fms2 = second[i];

                            if (!fms1.Equals(fms2))
                            {
                                fms1.ModifiedBy   = user;
                                fms1.ModifiedDate = modDate;
                            }
                        }
                        else
                        {
                            // This is a case where we don't have as many cloned objects as is now
                            // in ths object, so it will for sure need to be marked as modified.
                            fms1.ModifiedBy   = user;
                            fms1.ModifiedDate = modDate;
                        }
                    }

                    i++;
                }
            }
        }

        #endregion IComplexModel
    }
}
