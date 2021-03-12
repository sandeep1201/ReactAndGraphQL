using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TransportationSectionMethodBridgeConfig : BaseCommonModelConfig<TransportationSectionMethodBridge>
    {
        public TransportationSectionMethodBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.TransportationSection)
                .WithMany(p => p.TransportationSectionMethodBridges)
                .HasForeignKey(p => p.TransportationSectionId);

            HasOptional(p => p.TransportationType)
                .WithMany()
                .HasForeignKey(p => p.TransporationTypeId);

            #endregion

            #region Properties

            ToTable("TransportationSectionMethodBridge");

            Property(p => p.TransportationSectionId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.TransporationTypeId)
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
