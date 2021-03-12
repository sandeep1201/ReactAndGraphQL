using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAVerificationTypeBridgeConfig : BaseConfig<EAVerificationTypeBridge>
    {
        public EAVerificationTypeBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EaVerificationType)
                .WithMany()
                .HasForeignKey(p => p.VerificationTypeId);

            #endregion

            #region Properties

            ToTable("EAVerificationTypeBridge");

            Property(p => p.VerificationTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsIncome)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsAsset)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsVehicle)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsVehicleValue)
                .HasColumnType("bit")
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
                .IsOptional();

            #endregion
        }
    }
}
