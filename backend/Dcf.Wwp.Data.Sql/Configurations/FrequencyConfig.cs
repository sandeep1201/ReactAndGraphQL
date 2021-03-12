using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class FrequencyConfig : BaseCommonModelConfig<Frequency>
    {
        public FrequencyConfig()
        {
            #region Relationships

            HasMany(p => p.MRActivityScheduleFrequencyBridges)
                .WithOptional(p => p.MRFrequency)
                .HasForeignKey(p => p.MRFrequencyId);

            HasMany(p => p.WKActivityScheduleFrequencyBridges)
                .WithOptional(p => p.WKFrequency)
                .HasForeignKey(p => p.WKFrequencyId);

            #endregion

            #region Properties

            ToTable("Frequency");

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsRequired();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

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

            #endregion
        }
    }
}
