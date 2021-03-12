using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class BarrierSubtypeConfig : BaseCommonModelConfig<BarrierSubtype>
    {
        public BarrierSubtypeConfig()
        {
            #region Relationships

            HasOptional(p => p.BarrierType)
                .WithMany(p => p.BarrierSubtypes)
                .HasForeignKey(p => p.BarrierTypeId);

            HasMany(p => p.BarrierTypeBarrierSubTypeBridges)
                .WithOptional(p => p.BarrierSubtype)
                .HasForeignKey(p => p.BarrierSubTypeId);

            #endregion

            #region Properties

            ToTable("BarrierSubtype");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.DisablesOthersFlag)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.BarrierTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

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
