using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ParticipantContactInfoConfig : BaseConfig<ParticipantContactInfo>
    {
        public ParticipantContactInfoConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.ParticipantContactInfoes)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.CountyAndTribe)
                .WithMany()
                .HasForeignKey(p => p.CountyOfResidenceId);

            HasOptional(p => p.City)
                .WithMany(p => p.ParticipantContactInfoes)
                .HasForeignKey(p => p.CityAddressId);

            HasOptional(p => p.AlternateMailingAddress)
                .WithMany(p => p.ParticipantContactInfoes)
                .HasForeignKey(p => p.AlternateMailingAddressId);

            HasOptional(p => p.AddressVerificationTypeLookup)
                .WithMany(p => p.ParticipantContactInfoes)
                .HasForeignKey(p => p.AddressVerificationTypeLookupId);

            #endregion

            #region Properties

            ToTable("ParticipantContactInfo");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CountyOfResidenceId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StreetAddressPlaceId)
                .HasColumnType("varchar")
                .HasMaxLength(1024)
                .IsOptional();

            Property(p => p.ZipCode)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.CityAddressId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HomelessIndicator)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsHouseHoldMailingAddressSame)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.AlternateMailingAddressId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PrimaryPhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.CanTextPrimaryPhone)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CanLeaveVoiceMailPrimaryPhone)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SecondaryPhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.CanTextSecondaryPhone)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CanLeaveVoiceMailSecondaryPhone)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.EmailAddress)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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

            Property(p => p.AddressLine1)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.AddressLine2)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.AddressVerificationTypeLookupId)
                .HasColumnType("int")
                .IsOptional();

            #endregion
        }
    }
}
