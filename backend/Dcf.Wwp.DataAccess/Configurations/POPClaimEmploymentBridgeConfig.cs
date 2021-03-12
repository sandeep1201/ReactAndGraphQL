using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class POPClaimEmploymentBridgeConfig : BaseConfig<POPClaimEmploymentBridge>
    {
        public POPClaimEmploymentBridgeConfig()
        {
            #region Relationship

            HasRequired(p => p.POPClaim)
                .WithMany(p => p.POPClaimEmploymentBridges)
                .HasForeignKey(p => p.POPClaimId);

            HasRequired(p => p.EmploymentInformation)
                .WithMany(p => p.POPClaimEmploymentBridges)
                .HasForeignKey(p => p.EmploymentInformationId);

            #endregion

            #region Properties

            ToTable("POPClaimEmploymentBridge");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.POPClaimId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmploymentInformationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsPrimary)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.HoursWorked)
                .HasColumnType("decimal")
                .HasPrecision(5, 1)
                .IsRequired();

            Property(p => p.Earnings)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .IsRequired();

            #endregion
        }
    }
}
