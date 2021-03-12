using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class JobQueueConfig : BaseConfig<JobQueue>
    {
        public JobQueueConfig()
        {
            #region Relationships

            HasMany(p => p.JobQueueItems)
                .WithRequired(p => p.JobQueue)
                .HasForeignKey(p => p.JobQueueId);

            HasMany(p => p.JobQueueItemHistories)
                .WithRequired(p => p.JobQueue)
                .HasForeignKey(p => p.JobQueueId);

            #endregion

            #region Properties

            ToTable("JobQueue");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsOptional();

            Property(p => p.QueueType)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Partition)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ItemsToProcessConcurrently)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DefaultMaxRetries)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DefaultRetryTimeBuffer)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DefaultSleepTime)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsActive)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsSimulation)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsRequired();

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
