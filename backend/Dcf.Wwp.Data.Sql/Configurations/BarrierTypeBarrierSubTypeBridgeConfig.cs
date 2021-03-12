using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class BarrierTypeBarrierSubTypeBridgeConfig : BaseCommonModelConfig<BarrierTypeBarrierSubTypeBridge>
    {
        public BarrierTypeBarrierSubTypeBridgeConfig()
        {
            //TEST these relationships

            #region Relationships

            HasOptional(p => p.BarrierDetail)
                .WithMany(p => p.BarrierTypeBarrierSubTypeBridges)
                .HasForeignKey(p => p.BarrierDetailId);

            HasOptional(p => p.BarrierSubtype)
                .WithMany(p => p.BarrierTypeBarrierSubTypeBridges)
                .HasForeignKey(p => p.BarrierSubTypeId);

            #endregion

            #region Properties

            ToTable("BarrierTypeBarrierSubTypeBridge");

            Property(p => p.BarrierDetailId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BarrierSubTypeId)
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
