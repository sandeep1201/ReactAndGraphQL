using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAIPVReasonBridgeConfig : BaseConfig<EAIPVReasonBridge>
    {
        public EAIPVReasonBridgeConfig()
        {
            #region Relationship

            HasRequired(p => p.EaIpv)
                .WithMany(p => p.EaIpvReasonBridges)
                .HasForeignKey(p => p.IPVId);

            HasRequired(p => p.Reason)
                .WithMany()
                .HasForeignKey(p => p.ReasonId);

            #endregion

            #region Properties

            ToTable("EAIPVReasonBridge");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IPVId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ReasonId)
                .HasColumnType("int")
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
