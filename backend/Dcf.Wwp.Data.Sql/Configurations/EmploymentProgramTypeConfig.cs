using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmploymentProgramTypeConfig : BaseCommonModelConfig<EmploymentProgramType>
    {
        public EmploymentProgramTypeConfig()
        {
            #region Relationships

            //HasMany(p => p.EmploymentInformations)
            //    .WithRequired(p => p.EmploymentProgramType)
            //    .HasForeignKey(p => p.EmployerOfRecordTypeId);

            #endregion

            #region Properties

            ToTable("EmploymentProgramType");

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
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
