using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ActionNeededTaskConfig : BaseConfig<ActionNeededTask>
    {
        public ActionNeededTaskConfig()
        {
            #region Relationships

            HasOptional(p => p.ActionAssignee)
                .WithMany()
                .HasForeignKey(p => p.ActionAssigneeId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.ActionItem)
                .WithMany()
                .HasForeignKey(p => p.ActionItemId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.ActionNeeded)
                .WithMany()
                .HasForeignKey(p => p.ActionNeededId)
                .WillCascadeOnDelete(false);

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

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.FollowUpTask)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

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

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsRequired();

            #endregion
        }
    }
}
