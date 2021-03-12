using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActionNeededTaskConfig : BaseCommonModelConfig<ActionNeededTask>
    {
        public ActionNeededTaskConfig()
        {
            #region Relationships

            HasRequired(p => p.ActionNeeded)
                .WithMany(p => p.ActionNeededTasks)
                .HasForeignKey(p => p.ActionNeededId);

            HasOptional(p => p.ActionAssignee)
                .WithMany(p => p.ActionNeededTasks)
                .HasForeignKey(p => p.ActionAssigneeId);

            HasOptional(p => p.ActionItem)
                .WithMany(p => p.ActionNeededTasks)
                .HasForeignKey(p => p.ActionItemId);

            HasOptional(p => p.ActionPriority)
                .WithMany()
                .HasForeignKey(p => p.ActionPriorityId);

            #endregion

            #region Properties

            ToTable("ActionNeededTask");

            Property(p => p.ActionNeededId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActionAssigneeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ActionItemId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ActionPriorityId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.FollowUpTask)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.DueDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsNoDueDate)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.CompletionDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsNoCompletionDate)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.FollowUpTask)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
