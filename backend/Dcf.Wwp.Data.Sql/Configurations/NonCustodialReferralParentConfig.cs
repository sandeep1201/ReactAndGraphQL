using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NonCustodialReferralParentConfig : BaseConfig<NonCustodialReferralParent>
    {
        public NonCustodialReferralParentConfig()
        {
            #region Relationships

            HasRequired(p => p.NonCustodialReferralParentsSection)
                .WithMany(p => p.NonCustodialReferralParents)
                .HasForeignKey(p => p.NonCustodialReferralParentsSectionId);

            HasOptional(p => p.Contact)
                .WithMany(p => p.NonCustodialReferralParents)
                .HasForeignKey(p => p.ContactId);

            HasMany(p => p.NonCustodialReferralChilds)
                .WithRequired(p => p.NonCustodialReferralParent)
                .HasForeignKey(p => p.NonCustodialReferralParentId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.NonCustodialReferralParents)
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("NonCustodialReferralParent");

            Property(p => p.NonCustodialReferralParentsSectionId)
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

            Property(p => p.IsAvailableOrWorking)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.AvailableOrWorkingDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsInterestedInWorkProgram)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.InterestedInWorkProgramDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsContactKnownWithParent)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ContactId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
