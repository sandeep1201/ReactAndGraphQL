using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class SchoolGraduationStatusConfig : BaseCommonModelConfig<SchoolGraduationStatus>
    {
        public SchoolGraduationStatusConfig()
        {
            #region Relationships

            HasMany(p => p.EducationSections)
                .WithOptional(p => p.SchoolGraduationStatus)
                .HasForeignKey(p => p.SchoolGraduationStatusId);

            #endregion

            #region Properties

            ToTable("SchoolGraduationStatus");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
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
