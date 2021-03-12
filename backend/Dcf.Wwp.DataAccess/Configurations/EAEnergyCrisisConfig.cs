using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAEnergyCrisisConfig : BaseConfig<EAEnergyCrisis>
    {
        public EAEnergyCrisisConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaEnergyCrises)
                .HasForeignKey(p => p.RequestId);

            #endregion

            #region Properties

            ToTable("EAEnergyCrisis");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.InNeedForUtilities)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DifficultyForUtilityBill)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.ExistingAppliedHelp)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.HaveThreat)
                .HasColumnType("bit")
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
