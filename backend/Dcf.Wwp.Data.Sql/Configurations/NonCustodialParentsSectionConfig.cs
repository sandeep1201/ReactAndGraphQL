using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NonCustodialParentsSectionConfig : BaseCommonModelConfig<NonCustodialParentsSection>
    {
        public NonCustodialParentsSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.ChildSupportContact)
                .WithMany(p => p.NonCustodialParentsSections)
                .HasForeignKey(p => p.ChildSupportContactId);

            HasMany(p => p.NonCustodialCaretakers)
                .WithRequired(p => p.NonCustodialParentsSection)
                .HasForeignKey(p => p.NonCustodialParentsSectionId);

            #endregion

            #region Properties

            ToTable("NonCustodialParentsSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.HasChildren)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ChildSupportPayment)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.HasOwedChildSupport)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasInterestInChildServices)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsInterestedInReferralServices)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.InterestedInReferralServicesDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.Notes)
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

            Property(p => p.ChildSupportContactId)
                .HasColumnType("int")
                .IsOptional();

            #endregion
        }
    }
}
