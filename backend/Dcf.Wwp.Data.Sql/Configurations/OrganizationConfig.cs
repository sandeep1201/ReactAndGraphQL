using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class OrganizationConfig : BaseConfig<Organization>
    {
        public OrganizationConfig()
        {
            #region Relationships

            HasMany(p => p.ContractAreas)
                .WithOptional(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId);

            HasMany(p => p.Workers)
                .WithOptional(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId);

            HasMany(p => p.EnrolledProgramOrganizationPopulationTypeBridges)
                .WithOptional(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId);

            #endregion

            #region Properties

            ToTable("Organization");

            Property(p => p.EntsecAgencyCode)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsOptional();

            Property(p => p.AgencyName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.DB2AgencyName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ActivatedDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.InActivatedDate)
                .HasColumnType("date")
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
