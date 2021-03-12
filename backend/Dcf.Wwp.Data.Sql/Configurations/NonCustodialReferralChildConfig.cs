using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NonCustodialReferralChildConfig : BaseConfig<NonCustodialReferralChild>
    {
        public NonCustodialReferralChildConfig()
        {
            #region Relationships

            HasRequired(p => p.NonCustodialReferralParent)
                .WithMany(p => p.NonCustodialReferralChilds)
                .HasForeignKey(p => p.NonCustodialReferralParentId);

            HasOptional(p => p.ReferralContactInterval)
                .WithMany()
                .HasForeignKey(p => p.ReferralContactIntervalId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.NonCustodialReferralChilds)
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("NonCustodialReferralChild");

            Property(p => p.NonCustodialReferralParentId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsOptional();

            Property(p => p.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsOptional();

            Property(p => p.ReferralContactIntervalId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContactIntervalDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.HasChildSupportOrder)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ChildSupportOrderDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

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
