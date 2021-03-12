using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class LegalIssuesAssessmentSectionConfig : BaseCommonModelConfig<LegalIssuesAssessmentSection>
    {
        public LegalIssuesAssessmentSectionConfig()
        {
            #region Relationships

            HasMany(p => p.InformalAssessments)
                .WithOptional(p => p.LegalIssuesAssessmentSection)
                .HasForeignKey(p => p.LegalIssuesAssessmentSectionId);

            #endregion

            #region Properties

            ToTable("LegalIssuesAssessmentSection");

            Property(p => p.ReviewCompleted)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ActionDetails)
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
