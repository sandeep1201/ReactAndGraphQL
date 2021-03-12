using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class DeleteReasonConfig : BaseCommonModelConfig<DeleteReason>
    {
        public DeleteReasonConfig()
        {
            #region Relationships

            HasMany(p => p.TimeLimitExtensions)
                .WithOptional(p => p.DeleteReason)
                .HasForeignKey(p => p.DeleteReasonId);

            HasMany(p => p.DeleteReasonByRepeaters)
                .WithRequired(p => p.DeleteReason)
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("DeleteReason");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
