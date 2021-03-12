using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActivityConfig : BaseCommonModelConfig<Activity>
    {
        public ActivityConfig()
        {
            #region Relationships

            HasOptional(p => p.ActivityLocation)
                .WithMany()
                .HasForeignKey(p => p.ActivityLocationId);

            HasRequired(p => p.ActivityType)
                .WithMany()
                .HasForeignKey(p => p.ActivityTypeId);

            HasMany(p => p.ActivitySchedules)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.ActivityContactBridges)
                .WithOptional(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.EmployabilityPlanActivityBridges)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.NonSelfDirectedActivities)
                .WithRequired(p => p.Activity)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

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

            Property(p => p.StartDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.ActivityCompletionReasonId)
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
                .IsRequired();

            Property(p => p.ActivityCompletionReasonId)
                .HasColumnType("int")
                .IsOptional();

            #endregion
        }
    }
}
