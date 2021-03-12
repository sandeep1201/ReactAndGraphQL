using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActionAssigneeConfig : BaseCommonModelConfig<ActionAssignee>
    {
        public ActionAssigneeConfig()
        {
            #region Relationships

            HasMany(p => p.ActionNeededTasks)
                .WithOptional(p => p.ActionAssignee)
                .HasForeignKey(p => p.ActionAssigneeId);

            #endregion

            #region Properties

            ToTable("ActionAssignee");

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
