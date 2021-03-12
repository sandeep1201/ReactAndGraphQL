using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ElevatedAccessReasonConfig : BaseCommonModelConfig<ElevatedAccessReason>
    {
        public ElevatedAccessReasonConfig()
        {
            #region Relationships

            HasMany(p => p.ElevatedAccesses)
                .WithOptional(p => p.ElevatedAccessReason)
                .HasForeignKey(p => p.ElevatedAccessReasonId);

            #endregion

            #region Properties

            ToTable("ElevatedAccessReason");

            Property(p => p.Reason)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.SortOrder)
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
