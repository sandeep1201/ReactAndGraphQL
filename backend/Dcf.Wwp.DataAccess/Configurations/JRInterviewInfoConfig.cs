using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class JRInterviewInfoConfig : BaseConfig<JRInterviewInfo>
    {
        public JRInterviewInfoConfig()
        {
            #region Relationships

            HasRequired(p => p.JobReadiness)
                .WithMany()
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("JRInterviewInfo");

            Property(p => p.JobReadinessId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.LastInterviewDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.CanLookAtSocialMedia)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CanLookAtSocialMediaDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.HaveOutfit)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HaveOutfitDetails)
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
