using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ExamResultConfig : BaseCommonModelConfig<ExamResult>
    {
        public ExamResultConfig()
        {
            #region Relationships

            HasOptional(p => p.EducationExam)
                .WithMany()
                .HasForeignKey(p => p.EducationExamId);

            HasRequired(p => p.ExamSubjectType)
                .WithMany()
                .HasForeignKey(p => p.ExamSubjectTypeId);

            HasOptional(p => p.SPLType)
                .WithMany()
                .HasForeignKey(p => p.SPLTypeId);

            HasOptional(p => p.NRSType)
                .WithMany()
                .HasForeignKey(p => p.NRSTypeId);

            HasOptional(p => p.ExamEquivalencyType)
                .WithMany()
                .HasForeignKey(p => p.ExamEquivalencyTypeId);

            HasOptional(p => p.ExamPassType)
                .WithMany()
                .HasForeignKey(p => p.ExamPassTypeId);

            #endregion

            #region Properties

            ToTable("ExamResult");

            Property(p => p.EducationExamId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ExamSubjectTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.DatePassed)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Score)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.MaxScoreRange)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SPLTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.NRSTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Version)
                .HasColumnType("varchar")
                .HasMaxLength(75)
                .IsOptional();

            Property(p => p.ExamEquivalencyTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.GradeEquivalency)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsOptional();

            Property(p => p.ExamLevelType)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ExamPassTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Level)
                .HasColumnType("varchar")
                .HasMaxLength(75)
                .IsOptional();

            Property(p => p.Form)
                .HasColumnType("varchar")
                .HasMaxLength(4)
                .IsOptional();

            Property(p => p.CasasGradeEquivalency)
                .HasColumnType("varchar")
                .HasMaxLength(2)
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
