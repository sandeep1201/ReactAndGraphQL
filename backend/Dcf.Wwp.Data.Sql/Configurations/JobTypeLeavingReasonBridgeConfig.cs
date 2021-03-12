using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class JobTypeLeavingReasonBridgeConfig : BaseCommonModelConfig<JobTypeLeavingReasonBridge>
    {
        public JobTypeLeavingReasonBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.JobType)
                .WithMany(p => p.JobTypeLeavingReasonBridges)
                .HasForeignKey(p => p.JobTypeId);

            HasRequired(p => p.LeavingReason)
                .WithMany(p => p.JobTypeLeavingReasonBridges)
                .HasForeignKey(p => p.LeavingReasonId);

            #endregion

            #region Properties

            ToTable("JobTypeLeavingReasonBridge");

            Property(p => p.JobTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.LeavingReasonId)
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
                .IsRequired();

            #endregion
        }
    }
}
