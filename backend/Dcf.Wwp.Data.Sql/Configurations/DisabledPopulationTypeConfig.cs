using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class DisabledPopulationTypeConfig : BaseCommonModelConfig<DisabledPopulationType>
    {
        public DisabledPopulationTypeConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgramOrganizationPopulationTypeBridge)
                .WithMany(p => p.DisabledPopulationTypes)
                .HasForeignKey(p => p.EnrolledProgramOrganizationPopulationTypeBridgeId);

            HasRequired(p => p.PopulationType)
                .WithMany()
                .HasForeignKey(p => p.PopulationTypeId);

            #endregion

            #region Properties

            ToTable("DisabledPopulationType");

            Property(p => p.EnrolledProgramOrganizationPopulationTypeBridgeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PopulationTypeId)
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
