using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class POPClaimHighWageConfig : BaseConfig<POPClaimHighWage>
    {
        public POPClaimHighWageConfig()
        {
            #region Relationships

            HasRequired(p => p.Organization)
                .WithMany(p => p.POPClaimHighWages)
                .HasForeignKey(p => p.OrganizationId);

            #endregion

            #region Properties

            ToTable("POPClaimHighWage");

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.StartingWage)
                .HasColumnType("decimal")
                .HasPrecision(5, 2)
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
