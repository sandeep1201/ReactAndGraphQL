using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class POPClaimTypeConfig : BaseConfig<POPClaimType>
    {
        public POPClaimTypeConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            ToTable("POPClaimType");

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsRequired();

            Property(p => p.Description)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.MinimumHoursWorked)
                .HasColumnType("decimal")
                .HasPrecision(5, 1)
                .IsRequired();

            Property(p => p.MinimumEarnings)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsRequired();

            Property(p => p.IsSystemUseOnly)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.EffectiveDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.EndDate)
                .HasColumnType("date")
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
                .IsRequired();

            #endregion
        }
    }
}
