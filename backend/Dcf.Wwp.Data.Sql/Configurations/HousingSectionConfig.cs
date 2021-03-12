using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class HousingSectionConfig : BaseCommonModelConfig<HousingSection>
    {
        public HousingSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.HousingSituation)
                .WithMany()
                .HasForeignKey(p => p.HousingSituationId);

            HasMany(p => p.HousingHistories)
                .WithOptional(p => p.HousingSection)
                .HasForeignKey(p => p.HousingSectionId);

            #endregion

            #region Properties

            ToTable("HousingSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.HousingSituationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CurrentHousingDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.CurrentHousingBeginDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.CurrentHousingEndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.CurrentMonthlyAmount)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.IsCurrentAmountUnknown)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasCurrentEvictionRisk)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasBeenEvicted)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsCurrentMovingToHistory)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasUtilityDisconnectionRisk)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.UtilityDisconnectionRiskNotes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.HasDifficultyWorking)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DifficultyWorkingNotes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.OriginId)
                .HasColumnType("int")
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
