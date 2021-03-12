using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EARequestEmergencyTypeBridgeConfig : BaseConfig<EARequestEmergencyTypeBridge>
    {
        public EARequestEmergencyTypeBridgeConfig()
        {
            #region Relationship

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaRequestEmergencyTypeBridges)
                .HasForeignKey(p => p.RequestId);

            HasRequired(p => p.EaEmergencyType)
                .WithMany()
                .HasForeignKey(p => p.EmergencyTypeId);

            HasRequired(p => p.EaEmergencyTypeReason)
                .WithMany()
                .HasForeignKey(p => p.EmergencyTypeReasonId);

            #endregion

            #region Properties

            ToTable("EARequestEmergencyTypeBridge");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmergencyTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmergencyTypeReasonId)
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
