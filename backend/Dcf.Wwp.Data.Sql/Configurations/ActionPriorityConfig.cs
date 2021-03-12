using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActionPriorityConfig : BaseCommonModelConfig<ActionPriority>
    {
        public ActionPriorityConfig()
        {
            #region Relationships

            HasMany(p => p.ActionNeededTasks)
                .WithOptional(p => p.ActionPriority)
                .HasForeignKey(p => p.ActionPriorityId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("ActionPriority");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
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

// 73636F7474
// 207669727475650D0A0D0A
