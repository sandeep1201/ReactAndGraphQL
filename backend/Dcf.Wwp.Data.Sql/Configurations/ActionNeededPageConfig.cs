using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActionNeededPageConfig : BaseCommonModelConfig<ActionNeededPage>
    {
        public ActionNeededPageConfig()
        {
            #region Relationships

            HasMany(p => p.ActionNeededs)
                .WithRequired(p => p.ActionNeededPage)
                .HasForeignKey(p => p.ActionNeededPageId);

            HasMany(p => p.ActionNeededPageActionItemBridges)
                .WithRequired(p => p.ActionNeededPage)
                .HasForeignKey(p => p.ActionNeededPageId);

            #endregion

            #region Properties

            ToTable("ActionNeededPage");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(50)
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
