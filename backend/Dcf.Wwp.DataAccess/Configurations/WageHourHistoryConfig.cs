using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class WageHourHistoryConfig : BaseConfig<WageHourHistory>
    {
        public WageHourHistoryConfig()
        {
            #region Relationships

            HasOptional(p => p.WageHour)
                .WithMany(p => p.WageHourHistories)
                .HasForeignKey(p => p.WageHourId);

            #endregion

            #region Properties

            ToTable("WageHourHistory");

            Property(p => p.WageHourId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HourlySubsidyRate)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.EffectiveDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.PayTypeDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.AverageWeeklyHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 0)
                .IsOptional();

            Property(p => p.PayRate)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.PayRateIntervalId)
                .HasColumnType("int")
                .IsOptional();

            Property(e => e.ComputedWageRateUnit)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(e => e.ComputedWageRateValue)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

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
