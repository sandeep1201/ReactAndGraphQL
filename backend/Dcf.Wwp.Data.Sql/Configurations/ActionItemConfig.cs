using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActionItemConfig : BaseCommonModelConfig<ActionItem>
    {
        public ActionItemConfig()
        {
            #region Relationships

            HasMany(p => p.ActionNeededPageActionItemBridges)
                .WithRequired(p => p.ActionItem)
                .HasForeignKey(p => p.ActionItemId);

            HasMany(p => p.ActionNeededTasks)
                .WithOptional(p => p.ActionItem)
                .HasForeignKey(p => p.ActionItemId);

            #endregion

            #region Properties

            ToTable("ActionItem");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
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
