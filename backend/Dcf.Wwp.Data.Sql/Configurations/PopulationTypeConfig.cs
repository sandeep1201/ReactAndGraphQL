using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class PopulationTypeConfig : BaseCommonModelConfig<PopulationType>
    {
        public PopulationTypeConfig()
        {
            #region Relationships

            HasMany(p => p.DisabledPopulationTypes)
                .WithRequired(p => p.PopulationType)
                .HasForeignKey(p => p.PopulationTypeId);

            HasMany(p => p.EnrolledProgramOrganizationPopulationTypeBridges)
                .WithRequired(p => p.PopulationType)
                .HasForeignKey(p => p.PopulationTypeId);

            HasMany(p => p.RequestForAssistancePopulationTypeBridges)
                .WithOptional(p => p.PopulationType)
                .HasForeignKey(p => p.PopulationTypeId);

            #endregion

            #region Properties

            ToTable("PopulationType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
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
