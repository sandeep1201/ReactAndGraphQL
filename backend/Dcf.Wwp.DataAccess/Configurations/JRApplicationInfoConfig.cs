using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class JRApplicationInfoConfig : BaseConfig<JRApplicationInfo>
    {
        public JRApplicationInfoConfig()
        {
            #region Relationships

            HasRequired(p => p.JobReadiness)
                .WithMany()
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.NeedDocumentLookup)
                .WithMany()
                .HasForeignKey(p => p.NeedDocumentLookupId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("JRApplicationInfo");

            Property(p => p.JobReadinessId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CanSubmitOnline)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CanSubmitOnlineDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.HaveCurrentResume)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HaveCurrentResumeDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.HaveProfessionalReference)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HaveProfessionalReferenceDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.NeedDocumentLookupId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.NeedDocumentDetail)
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
