using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ActivityScheduleFrequencyBridgeConfig : BaseConfig<ActivityScheduleFrequencyBridge>
    {
        public ActivityScheduleFrequencyBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.ActivitySchedule)
                .WithMany(p => p.ActivityScheduleFrequencyBridges)
                .HasForeignKey(p => p.ActivityScheduleId)
                .WillCascadeOnDelete(true);

            HasOptional(p => p.WKFrequency)
                .WithMany()
                .HasForeignKey(p => p.WKFrequencyId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.MRFrequency)
                .WithMany()
                .HasForeignKey(p => p.MRFrequencyId)
                .WillCascadeOnDelete(false);

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
