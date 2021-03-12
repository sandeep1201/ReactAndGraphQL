using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EducationExamConfig : BaseConfig<EducationExam>
    {
        public EducationExamConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

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
