using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAAlternateMailingAddressConfig : BaseConfig<EAAlternateMailingAddress>
    {
        public EAAlternateMailingAddressConfig()
        {
            #region Relationships

            HasRequired(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityAddressId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EARequestContactInfoes)
                .WithOptional(p => p.EAAlternateMailingAddress)
                .HasForeignKey(p => p.AlternateMailingAddressId);

            HasRequired(p => p.AddressVerificationTypeLookup)
                .WithMany()
                .HasForeignKey(p => p.AddressVerificationTypeLookupId);

            HasMany(p => p.EaPayments)
                .WithOptional(p => p.EaAlternateMailingAddress)
                .HasForeignKey(p => p.MailingAddressId);

            HasMany(p => p.EaIpvs)
                .WithOptional(p => p.EaAlternateMailingAddress)
                .HasForeignKey(p => p.MailingAddressId);

            #endregion

            #region Properties

            ToTable("EAAlternateMailingAddress");

            Property(p => p.ZipCode)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();

            Property(p => p.CityAddressId)
                .HasColumnName("CityAddressId")
                .HasColumnType("int")
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
                .IsRequired();

            Property(p => p.AddressLine1)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsRequired();

            Property(p => p.AddressLine2)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.AddressVerificationTypeLookupId)
                .HasColumnType("int")
                .IsRequired();

            #endregion
        }
    }
}
