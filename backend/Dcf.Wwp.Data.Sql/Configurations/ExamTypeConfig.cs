using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ExamTypeConfig : BaseCommonModelConfig<ExamType>
    {
        public ExamTypeConfig()
        {
            #region Relationships

            HasMany(p => p.ExamSubjectMaxScoreTypes)
                .WithOptional(p => p.ExamType)
                .HasForeignKey(p => p.ExamTypeId);

            HasMany(p => p.ExamSubjectTypeBridges)
                .WithOptional(p => p.ExamType)
                .HasForeignKey(p => p.ExamTypeId);

            HasMany(p => p.EducationExams)
                .WithOptional(p => p.ExamType)
                .HasForeignKey(p => p.ExamTypeId);

            HasMany(p => p.ExamSubjectTypes)
                .WithOptional(p => p.ExamType)
                .HasForeignKey(p => p.ExamTypeId);

            #endregion

            #region Properties

            ToTable("ExamType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.FullName)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
