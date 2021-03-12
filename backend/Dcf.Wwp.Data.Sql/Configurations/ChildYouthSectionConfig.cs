using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ChildYouthSectionConfig : BaseCommonModelConfig<ChildYouthSection>
    {
        public ChildYouthSectionConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.ChildYouthSections)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.Contact)
                .WithMany(p => p.ChildYouthSections)
                .HasForeignKey(p => p.ChildWelfareContactId);

            HasOptional(p => p.YesNoUnknownLookup)
                .WithMany()
                .HasForeignKey(p => p.HasChildWelfareWorker);

            HasMany(p => p.ChildYouthSectionChilds)
                .WithOptional(p => p.ChildYouthSection)
                .HasForeignKey(p => p.ChildYouthSectionId);

            #endregion

            #region Properties

            ToTable("ChildYouthSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasChildren12OrUnder)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasChildrenOver12WithDisabilityInNeedOfChildCare)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasFutureChildCareNeed)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.FutureChildCareNeedNotes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.HasChildWelfareWorker)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ChildWelfareWorkerChildren)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.ChildWelfareWorkerPlanOrRequirements)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.ChildWelfareContactId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasWicBenefits)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsInHeadStart)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsInAfterSchoolOrSummerProgram)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.AfterSchoolProgramDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.IsInMentoringProgram)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.MentoringProgramDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.DidOrWillAgeOutOfFosterCare)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.FosterCareDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.IsSpecialNeedsProgramming)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SpecialNeedsProgrammingDetails)
                .HasColumnType("varchar")
                .HasMaxLength(500)
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
