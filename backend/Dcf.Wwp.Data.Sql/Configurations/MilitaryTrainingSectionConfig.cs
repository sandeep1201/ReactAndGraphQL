using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class MilitaryTrainingSectionConfig : BaseCommonModelConfig<MilitaryTrainingSection>
    {
        public MilitaryTrainingSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.MilitaryRank)
                .WithMany()
                .HasForeignKey(p => p.MilitaryRankId);

            HasOptional(p => p.MilitaryBranch)
                .WithMany()
                .HasForeignKey(p => p.MilitaryBranchId);

            HasOptional(p => p.MilitaryDischargeType)
                .WithMany()
                .HasForeignKey(p => p.MilitaryDischargeTypeId);

            HasOptional(p => p.IsEligibleForBenefitsPolarLookup)
                .WithMany()
                .HasForeignKey(p => p.PolarLookupId);

            #endregion

            #region Properties

            ToTable("MilitaryTrainingSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.DoesHaveTraining)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.MilitaryRankId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.MilitaryBranchId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Rate)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.YearsEnlisted)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EnlistmentDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.DischargeDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsCurrentlyEnlisted)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.MilitaryDischargeTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SkillsAndTraining)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.PolarLookupId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BenefitsDetails)
                .HasColumnType("varchar")
                .HasMaxLength(500)
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
