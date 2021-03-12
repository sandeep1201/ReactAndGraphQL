using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class WorkerTaskListConfig : BaseConfig<WorkerTaskList>
    {
        public WorkerTaskListConfig()
        {
            #region Relationships

            HasRequired(p => p.Worker)
                .WithMany()
                .HasForeignKey(p => p.WorkerId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.WorkerTaskCategory)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.ActionPriority)
                .WithMany()
                .HasForeignKey(p => p.ActionPriorityId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.WorkerTaskStatus)
                .WithMany()
                .HasForeignKey(p => p.WorkerTaskStatusId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("WorkerTaskList");


            Property(p => p.WorkerId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CategoryId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActionPriorityId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WorkerTaskStatusId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TaskDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.TaskDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsRequired();

            Property(p => p.StatusDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.DueDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsSystemGenerated)
                .HasColumnType("bit")
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
                .IsRequired();

            #endregion
        }
    }
}
