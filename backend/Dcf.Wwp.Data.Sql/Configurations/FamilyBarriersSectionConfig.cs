using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class FamilyBarriersSectionConfig : BaseCommonModelConfig<FamilyBarriersSection>
    {
        public FamilyBarriersSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.FamilyBarriersSections)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.ApplicationStatusType)
                .WithMany(p => p.FamilyBarriersSections)
                .HasForeignKey(p => p.SsiApplicationStatusId);

            HasOptional(p => p.FamilyBarriersSsiApplicationStatusDetail)
                .WithMany()
                .HasForeignKey(p => p.SsiApplicationStatusDetailsId);

            HasOptional(p => p.FamilyBarriersSsiApplicationDetail)
                .WithMany()
                .HasForeignKey(p => p.SsiApplicationDetailsId);

            HasOptional(p => p.Contact)
                .WithMany(p => p.FamilyBarriersSections)
                .HasForeignKey(p => p.SsiApplicationContactId);

            HasOptional(p => p.FamilyBarriersPastSsiDetail)
                .WithMany()
                .HasForeignKey(p => p.PastSsiDetailsId);

            HasOptional(p => p.FamilyBarriersDeniedSsiDetail)
                .WithMany()
                .HasForeignKey(p => p.DeniedSsiDetailsId);

            HasOptional(p => p.FamilyBarriersInterestedInLearningMoreSsiDetail)
                .WithMany()
                .HasForeignKey(p => p.InterestedInLearningMoreSsiDetailsId);

            HasOptional(p => p.FamilyBarriersAnyOneReceivingSsiDetail)
                .WithMany()
                .HasForeignKey(p => p.AnyoneReceivingSsiDetailsId);

            HasOptional(p => p.FamilyBarriersAnyoneApplyingForSsiDetail)
                .WithMany()
                .HasForeignKey(p => p.AnyoneApplyingForSsiDetailsId);

            HasOptional(p => p.FamilyBarriersConcernsAboutCaretakingResponsibilitiesDetail)
                .WithMany()
                .HasForeignKey(p => p.ConcernsAboutCaretakingResponsibilitiesDetailsId);

            HasOptional(p => p.FamilyBarriersHouseholdEngageRiskyActivitiesDetail)
                .WithMany()
                .HasForeignKey(p => p.HouseholdEngageInRiskyActivitiesDetailsId);

            HasOptional(p => p.FamilyBarriersChildrenHaveBehaviourProblemsDetail)
                .WithMany()
                .HasForeignKey(p => p.ChildrenHaveBehaviourProblemsDetailsId);

            HasOptional(p => p.FamilyBarriersChildrenAtRiskOfSchoolSuspensionDetail)
                .WithMany()
                .HasForeignKey(p => p.ChildrenAtRiskOfSchoolSuspensionDetailsId);

            HasOptional(p => p.FamilyBarriersAnyFamilyIssuesAffectWorkDetail)
                .WithMany()
                .HasForeignKey(p => p.AnyFamilyIssuesAffectWorkDetailsId);

            HasOptional(p => p.FamilyBarriersReasonForPastSsiDetail)
                .WithMany()
                .HasForeignKey(p => p.ReasonForPastSsiDetailsId);

            HasMany(p => p.FamilyMembers)
                .WithOptional(p => p.FamilyBarriersSection)
                .HasForeignKey(p => p.FamilyBarriersSectionId);

            #endregion

            #region Properties

            ToTable("FamilyBarriersSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.HasEverAppliedSsi)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsCurrentlyApplyingSsi)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SsiApplicationStatusId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SsiApplicationStatusDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SsiApplicationDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.SsiApplicationIsAnyoneHelping)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SsiApplicationDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SsiApplicationContactId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasReceivedPastSsi)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.PastSsiDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasDeniedSsi)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DeniedSsiDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.DeniedSsiDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsInterestedInLearningMoreSsi)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.InterestedInLearningMoreSsiDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasAnyoneAppliedForSsi)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsAnyoneReceivingSsi)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.AnyoneReceivingSsiDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsAnyoneApplyingForSsi)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.AnyoneApplyingForSsiDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasCaretakingResponsibilities)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasConcernsAboutCaretakingResponsibilities)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ConcernsAboutCaretakingResponsibilitiesDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DoesHouseholdEngageInRiskyActivities)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HouseholdEngageInRiskyActivitiesDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DoChildrenHaveBehaviourProblems)
                .HasColumnName("DoChildrenHaveBehaviourProblems")
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ChildrenHaveBehaviourProblemsDetailsId)
                .HasColumnName("ChildrenHaveBehaviourProblemsDetailsId")
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.AreChildrenAtRiskOfSchoolSuspension)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ChildrenAtRiskOfSchoolSuspensionDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.AreAnyFamilyIssuesAffectWork)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.AnyFamilyIssuesAffectWorkDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ReasonForPastSsiDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
