using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ParticipationPeriodSummaryConfig : BaseConfig<ParticipationPeriodSummary>
    {
        public ParticipationPeriodSummaryConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.ParParticipationPeriodSummaries)
                .HasForeignKey(p => p.ParticipantId);

            #endregion

            #region Properties

            ToTable("ParticipationPeriodSummary");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipationPeriodBeginDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.ParticipationPeriodEndDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.CaseNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.AppliedHours)
                .HasColumnType("decimal")
                .HasPrecision(5, 1)
                .IsOptional();

            Property(p => p.UnAppliedHours)
                .HasColumnType("decimal")
                .HasPrecision(5, 1)
                .IsOptional();

            Property(p => p.SanctionableHours)
                .HasColumnType("decimal")
                .HasPrecision(5, 1)
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
