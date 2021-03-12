using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NonCustodialParentsReferralSectionConfig : BaseCommonModelConfig<NonCustodialParentsReferralSection>
    {
        public NonCustodialParentsReferralSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.YesNoSkipLookup)
                .WithMany()
                .HasForeignKey(p => p.HasChildrenId);

            HasMany(p => p.NonCustodialReferralParents)
                .WithRequired(p => p.NonCustodialReferralParentsSection)
                .HasForeignKey(p => p.NonCustodialReferralParentsSectionId);

            #endregion

            #region Properties

            ToTable("NonCustodialParentsReferralSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.HasChildrenId)
                .HasColumnType("int")
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

            #endregion
        }
    }
}
