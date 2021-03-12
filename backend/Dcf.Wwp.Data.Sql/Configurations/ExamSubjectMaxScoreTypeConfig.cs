using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ExamSubjectMaxScoreTypeConfig : EntityTypeConfiguration<ExamSubjectMaxScoreType>
    {
        public ExamSubjectMaxScoreTypeConfig()
        {
            #region Relationships

            HasOptional(p => p.ExamType)
                .WithMany()
                .HasForeignKey(p => p.ExamTypeId);

            #endregion

            #region Properties

            ToTable("ExamSubjectMaxScoreType");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ExamTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ExamSubjectTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.MaxScore)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
