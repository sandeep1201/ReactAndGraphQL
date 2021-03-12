using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WorkerTaskListConfig : BaseCommonModelConfig<WorkerTaskList>
    {
        public WorkerTaskListConfig()
        {
            #region Relationships

            HasRequired(p => p.Worker)
                .WithMany(p => p.WorkerTaskLists)
                .HasForeignKey(p => p.WorkerId);

            HasRequired(p => p.Participant)
                .WithMany(p => p.WorkerTaskLists)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.WorkerTaskStatus)
                .WithMany()
                .HasForeignKey(p => p.WorkerTaskStatusId);

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
