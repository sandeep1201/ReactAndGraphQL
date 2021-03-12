using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class MilitaryTrainingAssessmentSectionConfig : BaseCommonModelConfig<MilitaryTrainingAssessmentSection>
    {
        public MilitaryTrainingAssessmentSectionConfig()
        {
            #region Relationships

            HasMany(p => p.InformalAssessments)
                .WithOptional(p => p.MilitaryTrainingAssessmentSection)
                .HasForeignKey(p => p.MilitaryTrainingAssessmentSectionId);

            #endregion

            #region Properties

            ToTable("MilitaryTrainingAssessmentSection");

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
