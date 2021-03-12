using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmployerOfRecordTypeConfig : BaseConfig<EmployerOfRecordType>
    {
        public EmployerOfRecordTypeConfig()
        {
            #region Relationships

            //HasMany(p => p.EmploymentInformations)
            //    .WithOptional(p => p.EmployerOfRecordType)
            //    .HasForeignKey(p => p.EmployerOfRecordTypeId);

            #endregion

            #region Properties

            ToTable("EmployerOfRecordType");

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
