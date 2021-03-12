using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class JobQueueItemHistoryConfig : BaseConfig<JobQueueItemHistory>
    {
        public JobQueueItemHistoryConfig()
        {
            #region Relationships

            HasRequired(p => p.JobQueue)
                .WithMany(p => p.JobQueueItemHistories)
                .HasForeignKey(p => p.JobQueueId);

            #endregion

            #region Properties

            ToTable("JobQueueItemHistory");

            Property(p => p.ExternalJobId)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsOptional();

            Property(p => p.JobQueueId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.JobStatus)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsReady)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsUrgent)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.RetryCount)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.MaxRetries)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.RetryTime)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.JobResult)
                .HasColumnType("varchar(max)")
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
