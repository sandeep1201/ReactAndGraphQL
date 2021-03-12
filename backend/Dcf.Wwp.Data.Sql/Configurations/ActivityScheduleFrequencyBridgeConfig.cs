using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActivityScheduleFrequencyBridgeConfig : BaseCommonModelConfig<ActivityScheduleFrequencyBridge>
    {
        public ActivityScheduleFrequencyBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.ActivitySchedule)
                .WithMany(p => p.ActivityScheduleFrequencyBridges)
                .HasForeignKey(p => p.ActivityScheduleId);

            HasOptional(p => p.WKFrequency)
                .WithMany()
                .HasForeignKey(p => p.WKFrequencyId);

            HasOptional(p => p.MRFrequency)
                .WithMany()
                .HasForeignKey(p => p.MRFrequencyId);

            #endregion

            #region Properties

            ToTable("ActivityScheduleFrequencyBridge");

            Property(p => p.ActivityScheduleId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WKFrequencyId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.MRFrequencyId)
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
