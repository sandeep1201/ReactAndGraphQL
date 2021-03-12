using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActivityScheduleConfig : BaseCommonModelConfig<ActivitySchedule>
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
                .HasForeignKey(p => p.FrequencyTypeId);

            HasOptional(p => p.EmployabilityPlan)
                .WithMany(p => p.ActivitySchedules)
                .HasForeignKey(p => p.EmployabilityPlanId);

            HasMany(p => p.ActivityScheduleFrequencyBridges)
                .WithOptional(p => p.ActivitySchedule)
                .HasForeignKey(p => p.ActivityScheduleId);

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
                .IsOptional();

            Property(p => p.FrequencyTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PlannedEndDate)
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
                .IsRequired();

            Property(p => p.ActualEndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.HoursPerDay)
                .HasColumnType("decimal") // I'd have made this a 'bigint'
                .HasPrecision(3, 1)       // and a .NET TimeSpan, and store the .Ticks value... but that's too forward thinking (reports, etc?) ~ lol
                .IsOptional();

            Property(p => p.EmployabilityPlanId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BeginTime)
                .HasColumnType("time")
                .IsOptional(); // was .IsRequired(false);   .IsOptional() is the homegrown extension method.

            Property(p => p.EndTime)
                .HasColumnType("time")
                .IsOptional();

            #endregion
        }
    }
}
