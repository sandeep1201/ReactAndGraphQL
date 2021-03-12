using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class RequestForAssistancePopulationTypeBridgeConfig : BaseCommonModelConfig<RequestForAssistancePopulationTypeBridge>
    {
        public RequestForAssistancePopulationTypeBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.RequestForAssistance)
                .WithMany(p => p.RequestForAssistancePopulationTypeBridges)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasOptional(p => p.PopulationType)
                .WithMany(p => p.RequestForAssistancePopulationTypeBridges)
                .HasForeignKey(p => p.PopulationTypeId);

            #endregion

            #region Properties

            ToTable("RequestForAssistancePopulationTypeBridge");

            Property(p => p.RequestForAssistanceId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PopulationTypeId)
                .HasColumnType("int")
                .IsOptional();

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
