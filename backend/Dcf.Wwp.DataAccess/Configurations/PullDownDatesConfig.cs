using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class PullDownDatesConfig : BaseConfig<PullDownDate>
    {
        public PullDownDatesConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            ToTable("PullDownDate");

            Property(p => p.BenefitYear)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.BenefitMonth)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PullDownDates)
                .HasColumnName("PullDownDate")
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.DelayedCycleDate)
                .HasColumnType("date")
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

            Property(p => p.NoOfDaysInPeriod)
                .HasColumnType("smallint")
                .IsRequired();

            #endregion
        }
    }
}
