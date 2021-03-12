using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActionNeededPageActionItemBridgeConfig : BaseCommonModelConfig<ActionNeededPageActionItemBridge>
    {
        public ActionNeededPageActionItemBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.ActionItem)
                .WithMany(p => p.ActionNeededPageActionItemBridges)
                .HasForeignKey(p => p.ActionItemId);

            HasRequired(p => p.ActionNeededPage)
                .WithMany(p => p.ActionNeededPageActionItemBridges)
                .HasForeignKey(p => p.ActionNeededPageId);

            #endregion

            #region Properties

            ToTable("ActionNeededPageActionItemBridge");

            Property(p => p.ActionNeededPageId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActionItemId)
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
