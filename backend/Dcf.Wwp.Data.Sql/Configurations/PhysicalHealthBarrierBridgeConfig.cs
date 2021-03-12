using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class PhysicalHealthBarrierBridgeConfig : BaseConfig<PhysicalHealthBarrierBridge>
    {
        public PhysicalHealthBarrierBridgeConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("PhysicalHealthBarrierBridge");

            Property(p => p.PhysicalHealthId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BarrierId)
                .HasColumnType("int")
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
