using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class JobReadinessConfig : BaseConfig<JobReadiness>
    {
        public JobReadinessConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.JrApplicationInfos)
                .WithRequired(p => p.JobReadiness)
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.JrContactInfos)
                .WithRequired(p => p.JobReadiness)
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.JrHistoryInfos)
                .WithRequired(p => p.JobReadiness)
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.JrInterviewInfos)
                .WithRequired(p => p.JobReadiness)
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.JrWorkPreferenceses)
                .WithRequired(p => p.JobReadiness)
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("JobReadiness");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

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
                .IsRequired();

            #endregion
        }
    }
}
