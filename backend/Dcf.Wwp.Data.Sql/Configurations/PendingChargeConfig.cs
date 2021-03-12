using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class PendingChargeConfig : BaseCommonModelConfig<PendingCharge>
    {
        public PendingChargeConfig()
        {
            #region Relationships

            HasOptional(p => p.LegalIssuesSection)
                .WithMany(p => p.PendingCharges)
                .HasForeignKey(p => p.LegalSectionId);

            HasOptional(p => p.ConvictionType)
                .WithMany()
                .HasForeignKey(p => p.ConvictionTypeID);

            #endregion

            #region Properties

            ToTable("PendingCharge");

            Property(p => p.LegalSectionId)
                .HasColumnName("LegalSectionId")
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ConvictionTypeID)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ChargeDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsUnknown)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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
                .IsOptional();

            #endregion
        }
    }
}
