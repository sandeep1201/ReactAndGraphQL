using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WageHourHistoryWageTypeBridgeConfig : BaseCommonModelConfig<WageHourHistoryWageTypeBridge>
    {
        public WageHourHistoryWageTypeBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.WageHourHistory)
                .WithMany(p => p.WageHourHistoryWageTypeBridges)
                .HasForeignKey(p => p.WageHourHistoryId);

            HasOptional(p => p.WageType)
                .WithMany()
                .HasForeignKey(p => p.WageTypeId);

            #endregion

            #region Properties

            ToTable("WageHourHistoryWageTypeBridge");

            Property(p => p.WageHourHistoryId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WageTypeId)
                .HasColumnType("int")
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
