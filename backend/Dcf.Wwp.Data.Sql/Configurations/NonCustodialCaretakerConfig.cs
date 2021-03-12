using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NonCustodialCaretakerConfig : BaseConfig<NonCustodialCaretaker>
    {
        public NonCustodialCaretakerConfig()
        {
            #region Relationships

            HasRequired(p => p.NonCustodialParentsSection)
                .WithMany(p => p.NonCustodialCaretakers)
                .HasForeignKey(p => p.NonCustodialParentsSectionId);

            HasOptional(p => p.NonCustodialParentRelationship)
                .WithMany()
                .HasForeignKey(p => p.NonCustodialParentRelationshipId);

            HasOptional(p => p.ContactInterval)
                .WithMany(p => p.NonCustodialCaretakers)
                .HasForeignKey(p => p.ContactIntervalId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.NonCustodialCaretakers)
                .HasForeignKey(p => p.DeleteReasonId);

            HasMany(p => p.NonCustodialChilds)
                .WithRequired(p => p.NonCustodialCaretaker)
                .HasForeignKey(p => p.NonCustodialCaretakerId);

            #endregion

            #region Properties

            ToTable("NonCustodialCaretaker");

            Property(p => p.NonCustodialParentsSectionId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsOptional();

            Property(p => p.IsFirstNameUnknown)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsOptional();

            Property(p => p.IsLastNameUnknown)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.NonCustodialParentRelationshipId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.RelationshipDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.ContactIntervalId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContactIntervalDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsRelationshipChangeRequested)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.RelationshipChangeRequestedDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsInterestedInRelationshipReferral)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.InterestedInRelationshipReferralDetails)
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
