using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ActivityConfig : BaseConfig<Activity>
    {
        public ActivityConfig()
        {
            #region Relationships

            HasRequired(p => p.ActivityType)
                .WithMany()
                .HasForeignKey(p => p.ActivityTypeId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.ActivityLocation)
                .WithMany()
                .HasForeignKey(p => p.ActivityLocationId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.ActivityCompletionReason)
                .WithMany()
                .HasForeignKey(p => p.ActivityCompletionReasonId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ActivitySchedules)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.ActivityContactBridges)
                .WithOptional(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.NonSelfDirectedActivities)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.EmploybilityPlanActivityBridges)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.ParticipationEntries)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.CFParticipationEntries)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ParticipationEntryHistories)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.POPClaimActivityBridges)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("Activity");

            Property(p => p.ActivityTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Description)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.ActivityLocationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Details)
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
                .IsRequired();

            Property(p => p.ActivityCompletionReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.StartDate)
                .HasColumnType("date")
                .IsOptional();

            #endregion
        }
    }
}
