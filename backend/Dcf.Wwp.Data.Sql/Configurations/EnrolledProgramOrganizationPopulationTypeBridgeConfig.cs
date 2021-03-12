using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EnrolledProgramOrganizationPopulationTypeBridgeConfig : BaseConfig<EnrolledProgramOrganizationPopulationTypeBridge>
    {
        public EnrolledProgramOrganizationPopulationTypeBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany(p => p.EnrolledProgramOrganizationPopulationTypeBridges)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasOptional(p => p.Organization)
                .WithMany(p => p.EnrolledProgramOrganizationPopulationTypeBridges)
                .HasForeignKey(p => p.OrganizationId);

            HasRequired(p => p.PopulationType)
                .WithMany(p => p.EnrolledProgramOrganizationPopulationTypeBridges)
                .HasForeignKey(p => p.PopulationTypeId);

            HasMany(p => p.DisabledPopulationTypes)
                .WithRequired(p => p.EnrolledProgramOrganizationPopulationTypeBridge)
                .HasForeignKey(p => p.EnrolledProgramOrganizationPopulationTypeBridgeId);

            #endregion

            #region Properties

            ToTable("EnrolledProgramOrganizationPopulationTypeBridge");

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PopulationTypeId)
                .HasColumnType("int")
                .IsRequired();

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
