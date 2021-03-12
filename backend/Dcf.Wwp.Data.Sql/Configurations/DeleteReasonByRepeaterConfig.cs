using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class DeleteReasonByRepeaterConfig : BaseCommonModelConfig<DeleteReasonByRepeater>
    {
        public DeleteReasonByRepeaterConfig()
        {
            #region Relationships

            HasRequired(p => p.DeleteReason)
                .WithMany()
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("DeleteReasonByRepeater");

            Property(p => p.Repeater)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsRequired();

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
