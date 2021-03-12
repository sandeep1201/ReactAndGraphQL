using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ActivityScheduleConfig : BaseConfig<ActivitySchedule>
    {
        public ActivityScheduleConfig()
        {
            #region Relationships

            HasRequired(p => p.Activity)
                .WithMany(p => p.ActivitySchedules)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

            HasOptional(p => p.FrequencyType)
                .WithMany()
                .HasForeignKey(p => p.FrequencyTypeId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ActivityScheduleFrequencyBridges)
                .WithOptional(p => p.ActivitySchedule)
                .HasForeignKey(p => p.ActivityScheduleId)
                .WillCascadeOnDelete(true);

            HasOptional(p => p.EmployabilityPlan)
                .WithMany()
                .HasForeignKey(p => p.EmployabilityPlanId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("ActivitySchedule");

            Property(p => p.ActivityId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.StartDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.IsRecurring)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.FrequencyTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PlannedEndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.ActualEndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.HoursPerDay)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
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

            Property(p => p.EmployabilityPlanId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BeginTime)
                .HasColumnType("time")
                .IsOptional();

            Property(p => p.EndTime)
                .HasColumnType("time")
                .IsOptional();

            #endregion
        }
    }
}
