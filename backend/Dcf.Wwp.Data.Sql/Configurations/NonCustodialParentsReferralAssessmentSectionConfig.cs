using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NonCustodialParentsReferralAssessmentSectionConfig : BaseCommonModelConfig<NonCustodialParentsReferralAssessmentSection>
    {
        public NonCustodialParentsReferralAssessmentSectionConfig()
        {
            #region Relationships

            HasMany(p => p.InformalAssessments)
                .WithOptional(p => p.NonCustodialParentsReferralAssessmentSection)
                .HasForeignKey(p => p.NonCustodialParentsReferralAssessmentSectionId);

            #endregion

            #region Properties

            ToTable("NonCustodialParentsReferralAssessmentSection");

            Property(p => p.ReviewCompleted)
                .HasColumnType("bit")
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
