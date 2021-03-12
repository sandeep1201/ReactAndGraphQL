using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAAssetsConfig : BaseConfig<EAAssets>
    {
        public EAAssetsConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaAssetses)
                .HasForeignKey(p => p.RequestId);

            HasRequired(p => p.EaVerificationType)
                .WithMany()
                .HasForeignKey(p => p.VerificationTypeId);

            HasRequired(p => p.Participant)
                .WithMany(p => p.EaAssetses)
                .HasForeignKey(p => p.AssetOwner);

            #endregion

            #region Properties

            ToTable("EAAssets");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.AssetType)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.CurrentValue)
                .HasColumnType("decimal")
                .HasPrecision(8, 2)
                .IsOptional();

            Property(p => p.VerificationTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.AssetOwner)
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
