using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WageHourWageTypeBridgeConfig : BaseCommonModelConfig<WageHourWageTypeBridge>
    {
        public WageHourWageTypeBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.WageHour)
                .WithMany(p => p.WageHourWageTypeBridges)
                .HasForeignKey(p => p.WageHourId);

            HasOptional(p => p.WageType)
                .WithMany()
                .HasForeignKey(p => p.WageTypeId);

            #endregion

            #region Properties

            ToTable("WageHourWageTypeBridge");

            Property(p => p.WageHourId)
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
