using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ChildYouthSupportsAssessmentSectionConfig : BaseCommonModelConfig<ChildYouthSupportsAssessmentSection>
    {
        public ChildYouthSupportsAssessmentSectionConfig()
        {
            #region Relationships

            HasMany(p => p.InformalAssessments)
                .WithOptional(p => p.ChildYouthSupportsAssessmentSection)
                .HasForeignKey(p => p.ChildYouthSupportsAssessmentSectionId);

            #endregion

            #region Properties

            ToTable("ChildYouthSupportsAssessmentSection");

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
