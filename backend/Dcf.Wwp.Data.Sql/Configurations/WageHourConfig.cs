using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WageHourConfig : BaseCommonModelConfig<WageHour>
    {
        public WageHourConfig()
        {
            #region Relationships

            HasOptional(p => p.CurrentPayIntervalType)
                .WithMany()
                .HasForeignKey(p => p.CurrentPayRateIntervalId);

            HasOptional(p => p.BeginRateIntervalType)
                .WithMany()
                .HasForeignKey(p => p.PastBeginPayRateIntervalId);

            HasOptional(p => p.EndRateIntervalType)
                .WithMany()
                .HasForeignKey(p => p.PastEndPayRateIntervalId);

            HasMany(p => p.EmploymentInformations)
                .WithOptional(p => p.WageHour)
                .HasForeignKey(p => p.WageHoursId);

            HasMany(p => p.WageHourHistories)
                .WithOptional(p => p.WageHour)
                .HasForeignKey(p => p.WageHourId);

            HasMany(p => p.WageHourWageTypeBridges)
                .WithOptional(p => p.WageHour)
                .HasForeignKey(p => p.WageHourId);

            #endregion

            #region Properties

            ToTable("WageHour");

            Property(p => p.CurrentEffectiveDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.CurrentPayTypeDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.CurrentAverageWeeklyHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 0)
                .IsOptional();

            Property(p => p.CurrentPayRate)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.CurrentPayRateIntervalId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CurrentHourlySubsidyRate)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.PastBeginPayRate)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.PastBeginPayRateIntervalId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PastEndPayRate)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.PastEndPayRateIntervalId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsUnchangedPastPayRateIndicator)
                .HasColumnType("bit")
                .IsOptional();

            Property(e => e.ComputedCurrentWageRateUnit)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();
            //.IsUnicode(false)
            //.HasComputedColumnSql("([wwp].[GetComputedWageRateUnit]([CurrentPayRateIntervalId]))");

            Property(e => e.ComputedCurrentWageRateValue)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();
            //.HasComputedColumnSql("([wwp].[GetComputedWageRateValue]([CurrentPayRate],[CurrentPayRateIntervalId]))");

            Property(e => e.ComputedPastEndWageRateUnit)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();
            //.IsUnicode(false)
            //.HasComputedColumnSql("([wwp].[GetComputedWageRateUnit]([PastEndPayRateIntervalId]))");

            Property(e => e.ComputedPastEndWageRateValue)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();
            //.HasComputedColumnSql("([wwp].[GetComputedWageRateValue]([PastEndPayRate],[PastEndPayRateIntervalId]))");

            Property(e => e.WorkSiteContribution)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
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
