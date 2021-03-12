using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAVehiclesConfig : BaseConfig<EAVehicles>
    {
        public EAVehiclesConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaVehicleses)
                .HasForeignKey(p => p.RequestId);

            HasRequired(p => p.EaOwnershipVerificationType)
                .WithMany()
                .HasForeignKey(p => p.OwnershipVerificationTypeId);

            HasRequired(p => p.EaVehicleValueVerificationType)
                .WithMany()
                .HasForeignKey(p => p.VehicleValueVerificationTypeId);

            HasRequired(p => p.EaOwedVerificationType)
                .WithMany()
                .HasForeignKey(p => p.OwedVerificationTypeId);

            HasRequired(p => p.Participant)
                .WithMany(p => p.EaVehicleses)
                .HasForeignKey(p => p.VehicleOwner);

            #endregion

            #region Properties

            ToTable("EAVehicles");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.VehicleType)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.VehicleValue)
                .HasColumnType("decimal")
                .HasPrecision(8, 2)
                .IsOptional();

            Property(p => p.AmountOwed)
                .HasColumnType("decimal")
                .HasPrecision(8, 2)
                .IsOptional();

            Property(p => p.VehicleEquity)
                .HasColumnType("decimal")
                .HasPrecision(8, 2)
                .IsOptional();

            Property(p => p.OwnershipVerificationTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.VehicleValueVerificationTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OwedVerificationTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.VehicleOwner)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            #endregion
        }
    }
}
