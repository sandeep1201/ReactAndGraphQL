using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class CFParticipationEntryConfig : BaseConfig<CFParticipationEntry>
    {
        public CFParticipationEntryConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.CFParticipationEntries)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.EmployabilityPlan)
                .WithMany(p => p.CFParticipationEntries)
                .HasForeignKey(p => p.EPId);

            HasRequired(p => p.Activity)
                .WithMany(p => p.CFParticipationEntries)
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

            #endregion

            #region Properties

            ToTable("CFParticipationEntry");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EPId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActivityId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipationDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.ScheduledHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsRequired();

            Property(p => p.DidParticipate)
                .HasColumnType("bit")
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
