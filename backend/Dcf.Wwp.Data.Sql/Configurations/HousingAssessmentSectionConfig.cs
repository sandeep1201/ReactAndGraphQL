using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class HousingAssessmentSectionConfig : BaseCommonModelConfig<HousingAssessmentSection>
    {
        public HousingAssessmentSectionConfig()
        {
            #region Relationships

            HasMany(p => p.InformalAssessments) // this relationship is backwards (inverted, there's only ever one...smh)
                .WithOptional(p => p.HousingAssessmentSection)
                .HasForeignKey(p => p.HousingAssessmentSectionId);

            #endregion

            #region Properties

            ToTable("HousingAssessmentSection");

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
