using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class JRHistoryInfoConfig : BaseConfig<JRHistoryInfo>
    {
        public JRHistoryInfoConfig()
        {
            #region Relationships

            HasRequired(p => p.JobReadiness)
                .WithMany()
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("JRHistoryInfo");

            Property(p => p.JobReadinessId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.LastJobDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.AccomplishmentDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.StrengthDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.AreasNeedImprove)
                .HasColumnType("varchar")
                .HasMaxLength(380)
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
