using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmployerOfRecordInformationConfig : BaseCommonModelConfig<EmployerOfRecordInformation>
    {
        public EmployerOfRecordInformationConfig()
        {
            #region Relationships

            HasRequired(p => p.EmploymentInformation)
                .WithMany(p => p.EmployerOfRecordInformations)
                .HasForeignKey(p => p.EmploymentInformationId);

            HasOptional(p => p.City)
                .WithMany(p => p.EmployerOfRecordInformations)
                .HasForeignKey(p => p.CityId);

            HasOptional(p => p.JobSector)
                .WithMany(p => p.EmployerOfRecordInformations)
                .HasForeignKey(p => p.JobSectorId);

            HasOptional(p => p.Contact)
                .WithMany(p => p.EmployerOfRecordInformations)
                .HasForeignKey(p => p.ContactId);

            #endregion

            #region Properties

            ToTable("EmployerOfRecordInformation");

            Property(p => p.EmploymentInformationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CompanyName)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.Fein)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.StreetAddress)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.ZipAddress)
                .HasColumnType("varchar")
                .HasMaxLength(9)
                .IsOptional();

            Property(p => p.CityId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.JobSectorId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContactId)
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
