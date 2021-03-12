using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class BarrierDetailContactBridgeConfig : BaseCommonModelConfig<BarrierDetailContactBridge>
    {
        public BarrierDetailContactBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.BarrierDetail)
                .WithMany(p => p.BarrierDetailContactBridges)
                .HasForeignKey(p => p.BarrierDetailId);

            HasOptional(p => p.Contact)
                .WithMany(p => p.BarrierDetailContactBridges)
                .HasForeignKey(p => p.ContactId);

            #endregion

            #region Properties

            ToTable("BarrierDetailContactBridge");

            Property(p => p.BarrierDetailId)
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
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
