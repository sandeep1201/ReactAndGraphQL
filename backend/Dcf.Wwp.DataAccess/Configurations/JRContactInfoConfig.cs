using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class JRContactInfoConfig : BaseConfig<JRContactInfo>
    {
        public JRContactInfoConfig()
        {
            #region Relationships

            HasRequired(p => p.JobReadiness)
                .WithMany()
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("JRContactInfo");

            Property(p => p.JobReadinessId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CanYourPhoneNumberUsed)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.PhoneNumberDetails)
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsOptional();

            Property(p => p.HaveAccessToVoiceMailOrTextMessages)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.VoiceOrTextMessageDetails)
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsOptional();

            Property(p => p.HaveEmailAddress)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.EmailAddressDetails)
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsOptional();

            Property(p => p.HaveAccessDailyToEmail)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.AccessEmailDailyDetails)
                .HasColumnType("varchar")
                .HasMaxLength(120)
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
