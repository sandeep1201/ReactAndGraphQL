using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ParticipationEntryConfig : BaseConfig<ParticipationEntry>
    {
        public ParticipationEntryConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.ParticipationEntries)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.EmployabilityPlan)
                .WithMany(p => p.ParticipationEntries)
                .HasForeignKey(p => p.EPId);

            HasRequired(p => p.Activity)
                .WithMany(p => p.ParticipationEntries)
                .HasForeignKey(p => p.ActivityId);

            HasOptional(p => p.NonParticipationReason)
                .WithMany()
                .HasForeignKey(p => p.NonParticipationReasonId);

            HasOptional(p => p.GoodCauseGrantedReason)
                .WithMany()
                .HasForeignKey(p => p.GoodCauseGrantedReasonId);

            HasOptional(p => p.GoodCauseDeniedReason)
                .WithMany()
                .HasForeignKey(p => p.GoodCauseDeniedReasonId);

            HasOptional(p => p.PlacementType)
                .WithMany()
                .HasForeignKey(p => p.PlacementTypeId);

            HasMany(p => p.ParticipationMakeUpEntries)
                .WithRequired(p => p.ParticipationEntry)
                .HasForeignKey(p => p.ParticipationEntryId)
                .WillCascadeOnDelete(true);

            #endregion

            #region Properties

            ToTable("ParticipationEntry");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EPId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActivityId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CaseNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.ParticipationDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.ScheduledHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsRequired();

            Property(p => p.ReportedHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsOptional();

            Property(p => p.TotalMakeupHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsOptional();

            Property(p => p.ParticipatedHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsOptional();

            Property(p => p.NonParticipatedHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsOptional();

            Property(p => p.GoodCausedHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsOptional();

            Property(p => p.NonParticipationReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.NonParticipationReasonDetails)
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsOptional();

            Property(p => p.GoodCauseGranted)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.GoodCauseGrantedReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.GoodCauseDeniedReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.GoodCauseReasonDetails)
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsOptional();

            Property(p => p.PlacementTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.FormalAssessmentExists)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HoursSanctionable)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsProcessed)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ProcessedDate)
                .HasColumnType("date")
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
