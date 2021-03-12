using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class SchoolGradeLevelConfig : BaseCommonModelConfig<SchoolGradeLevel>
    {
        public SchoolGradeLevelConfig()
        {
            #region Relationships

            HasMany(p => p.EducationSections)
                .WithOptional(p => p.SchoolGradeLevel)
                .HasForeignKey(p => p.LastGradeLevelCompletedId);

            #endregion

            #region Properties

            ToTable("SchoolGradeLevel");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.Grade)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

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
