using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TimeLimitTypeConfig : BaseCommonModelConfig<TimeLimitType>
    {
        public TimeLimitTypeConfig()
        {
            #region Relationships

            // these are already defined on the other ends of the relationships. 

            // HasMany(p => p.ApprovalReasons)
            //     .WithMany(p => p.TimeLimitTypes)
            //     .Map(p =>
            //          {
            //              p.MapLeftKey("ApprovalReasonId"); // NOTE: the order here MUST match the column order in the table.
            //              p.MapRightKey("TimelimitTypeId");
            //              p.ToTable("ApprovalReasonsMap");
            //          });
            //
            // HasMany(p => p.DenialReasons)
            //     .WithMany(p => p.TimeLimitTypes)
            //     .Map(p =>
            //          {
            //              p.MapLeftKey("DenialReasonId"); // NOTE: the order here MUST match the column order in the table.
            //              p.MapRightKey("TimelimitTypeId");
            //              p.ToTable("DenialReasonsMap");
            //          });

            HasMany(p => p.TimeLimitExtensions)
                .WithOptional(p => p.TimeLimitType)
                .HasForeignKey(p => p.TimeLimitTypeId);

            HasMany(p => p.TimeLimits)
                .WithOptional(p => p.TimeLimitType)
                .HasForeignKey(p => p.TimeLimitTypeId);

            #endregion

            #region Properties

            ToTable("TimeLimitType");

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
