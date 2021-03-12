using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WorkProgramAssessmentSectionConfig : BaseCommonModelConfig<WorkProgramAssessmentSection>
    {
        public WorkProgramAssessmentSectionConfig()
        {
            #region Relationships

            HasMany(p => p.InformalAssessments)
                .WithOptional(p => p.WorkProgramAssessmentSection)
                .HasForeignKey(p => p.WorkProgramAssessmentSectionId);

            #endregion

            #region Properties

            ToTable("WorkProgramAssessmentSection");

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
