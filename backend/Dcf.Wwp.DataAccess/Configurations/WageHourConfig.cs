using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class WageHourConfig : BaseConfig<WageHour>
    {
        public WageHourConfig()
        {
            #region Relationships

            HasMany(p => p.EmploymentInformations)
                .WithRequired(p => p.WageHour)
                .HasForeignKey(p => p.WageHoursId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("WageHour");

            Property(p => p.CurrentEffectiveDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.CurrentPayTypeDetails)
                .HasColumnType("varchar")
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

            Property(p => p.PastEndPayRateIntervalId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PastEndPayRate)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.IsUnchangedPastPayRateIndicator)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ComputedCurrentWageRateUnit)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ComputedPastEndWageRateUnit)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ComputedCurrentWageRateValue)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.ComputedPastEndWageRateValue)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.WorkSiteContribution)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            #endregion
        }
    }
}
