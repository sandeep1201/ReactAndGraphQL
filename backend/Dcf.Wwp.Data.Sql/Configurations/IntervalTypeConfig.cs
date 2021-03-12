using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class IntervalTypeConfig : BaseCommonModelConfig<IntervalType>
    {
        public IntervalTypeConfig()
        {
            #region Relationships

            HasMany(p => p.BeginRateIntervalTypes)
                .WithOptional(p => p.BeginRateIntervalType)
                .HasForeignKey(p => p.PastBeginPayRateIntervalId);

            HasMany(p => p.CurrentPayRateIntervalTypes)
                .WithOptional(p => p.CurrentPayIntervalType)
                .HasForeignKey(p => p.CurrentPayRateIntervalId);

            HasMany(p => p.EndRateIntervalTypes)
                .WithOptional(p => p.EndRateIntervalType)
                .HasForeignKey(p => p.PastEndPayRateIntervalId);

            HasMany(p => p.WageHourHistories)
                .WithOptional(p => p.IntervalType)
                .HasForeignKey(p => p.PayRateIntervalId);

            #endregion

            #region Properties

            ToTable("IntervalType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
