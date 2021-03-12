using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class AlternateMailingAddressConfig : BaseConfig<AlternateMailingAddress>
    {
        public AlternateMailingAddressConfig()
        {
            #region Relationships

            HasOptional(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityAddressId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.State)
                .WithMany()
                .HasForeignKey(p => p.StateId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ParticipantContactInfoes)
                .WithOptional(p => p.AlternateMailingAddress)
                .HasForeignKey(p => p.AlternateMailingAddressId);

            HasOptional(p => p.AddressVerificationTypeLookup)
                .WithMany(p => p.AlternateMailingAddresses)
                .HasForeignKey(p => p.AddressVerificationTypeLookupId);

            #endregion

            #region Properties

            ToTable("AlternateMailingAddress");

            Property(p => p.StreetAddressPlaceId)
                .HasColumnType("varchar")
                .HasMaxLength(1024)
                .IsOptional();

            Property(p => p.ZipCode)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.CityAddressId)
                .HasColumnName("CityAddressId")
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StateId)
                .HasColumnType("int")
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
