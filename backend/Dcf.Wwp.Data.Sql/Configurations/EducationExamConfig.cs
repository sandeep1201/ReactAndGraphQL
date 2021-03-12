using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EducationExamConfig : BaseCommonModelConfig<EducationExam>
    {
        public EducationExamConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.ExamType)
                .WithMany()
                .HasForeignKey(p => p.ExamTypeId);

            HasMany(p => p.ExamResults)
                .WithOptional(p => p.EducationExam)
                .HasForeignKey(p => p.EducationExamId);

            #endregion

            #region Properties

            ToTable("EducationExam");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ExamTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DateTaken)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(400)
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
