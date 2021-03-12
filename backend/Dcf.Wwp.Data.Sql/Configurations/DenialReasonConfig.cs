using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class DenialReasonConfig : BaseCommonModelConfig<DenialReason>
    {
        public DenialReasonConfig()
        {
            #region Relationships

            HasMany(p => p.TimeLimitTypes)
                .WithMany(p => p.DenialReasons)
                .Map(p =>
                     {
                         p.MapLeftKey("DenialReasonId"); // NOTE: the order here MUST match the column order in the table.
                         p.MapRightKey("TimelimitTypeId");
                         p.ToTable("DenialReasonsMap");
                     });

            HasMany(p => p.TimeLimitExtensions)
                .WithOptional(p => p.DenialReason)
                .HasForeignKey(p => p.DenialReasonId);

            #endregion

            #region Properties

            ToTable("DenialReason");

            Property(p => p.Code)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsOptional();

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
