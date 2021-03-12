using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class BarrierTypeConfig : BaseCommonModelConfig<BarrierType>
    {
        public BarrierTypeConfig()
        {
            #region Relationships

            HasMany(p => p.BarrierDetails)
                .WithOptional(p => p.BarrierType)
                .HasForeignKey(p => p.BarrierTypeId);

            HasMany(p => p.BarrierSubtypes)
                .WithOptional(p => p.BarrierType)
                .HasForeignKey(p => p.BarrierTypeId);

            #endregion

            #region Properties

            ToTable("BarrierType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.IsRequired)
                .HasColumnType("bit")
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
