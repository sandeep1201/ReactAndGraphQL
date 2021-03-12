using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ApprovalReasonConfig : BaseCommonModelConfig<ApprovalReason>
    {
        public ApprovalReasonConfig()
        {
            #region Relationships

            HasMany(p => p.TimeLimitExtensions)
                .WithOptional(p => p.ApprovalReason)
                .HasForeignKey(p => p.ApprovalReasonId);

            HasMany(p => p.TimeLimitTypes)
                .WithMany(p => p.ApprovalReasons)
                .Map(p =>
                     {
                         p.MapLeftKey("ApprovalReasonId"); // NOTE: the order here MUST match the column order in the table.
                         p.MapRightKey("TimelimitTypeId");
                         p.ToTable("ApprovalReasonsMap");
                     });

            #endregion

            #region Properties

            ToTable("ApprovalReason");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.Code)
                .HasColumnType("char")
                .HasMaxLength(4)
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
