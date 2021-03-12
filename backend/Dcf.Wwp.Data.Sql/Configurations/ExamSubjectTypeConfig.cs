using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ExamSubjectTypeConfig : BaseConfig<ExamSubjectType>
    {
        public ExamSubjectTypeConfig()
        {
            #region Relationships

            HasOptional(p => p.ExamType)
                .WithMany()
                .HasForeignKey(p => p.ExamTypeId);

            HasMany(p => p.ExamResults)
                .WithRequired(p => p.ExamSubjectType)
                .HasForeignKey(p => p.ExamSubjectTypeId);

            HasMany(p => p.ExamSubjectTypeBridges)
                .WithOptional(p => p.ExamSubjectType)
                .HasForeignKey(p => p.ExamSubjectTypeId);

            #endregion

            #region Properties

            ToTable("ExamSubjectType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ExamTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

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
