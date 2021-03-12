using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ContractAreaConfig : BaseConfig<ContractArea>
    {
        public ContractAreaConfig()
        {
            #region Relationships

            HasOptional(p => p.Organization)
                .WithMany(p => p.ContractAreas)
                .HasForeignKey(p => p.OrganizationId);

            HasOptional(p => p.EnrolledProgram)
                .WithMany(p => p.ContractAreas)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasMany(p => p.Offices)
                .WithOptional(p => p.ContractArea)
                .HasForeignKey(p => p.ContractAreaId);

            HasMany(p => p.AssociatedOrganizations)
                .WithRequired(p => p.ContractArea)
                .HasForeignKey(p => p.ContractAreaId);

            #endregion

            #region Properties

            ToTable("ContractArea");

            Property(p => p.ContractAreaName)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
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
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
