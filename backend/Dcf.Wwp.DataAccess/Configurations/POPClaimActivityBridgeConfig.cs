using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class POPClaimActivityBridgeConfig : BaseConfig<POPClaimActivityBridge>
    {
        public POPClaimActivityBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.POPClaim)
                .WithMany(p => p.POPClaimActivityBridges)
                .HasForeignKey(p => p.POPClaimId);

            HasRequired(p => p.Activity)
                .WithMany(p => p.POPClaimActivityBridges)
                .HasForeignKey(p => p.ActivityId);

            #endregion

            #region Properties

            ToTable("POPClaimActivityBridge");

            Property(p => p.POPClaimId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActivityId)
                .HasColumnType("int")
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

            #endregion
        }
    }
}
