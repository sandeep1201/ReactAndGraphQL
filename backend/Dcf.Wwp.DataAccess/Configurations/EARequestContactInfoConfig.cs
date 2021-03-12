using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EARequestContactInfoConfig : BaseConfig<EARequestContactInfo>
    {
        public EARequestContactInfoConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaRequestContactInfos)
                .HasForeignKey(p => p.RequestId);

            HasOptional(p => p.CountyAndTribe)
                .WithMany()
                .HasForeignKey(p => p.CountyOfResidenceId);

            HasOptional(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityAddressId);

            HasOptional(p => p.EAAlternateMailingAddress)
                .WithMany(p => p.EARequestContactInfoes)
                .HasForeignKey(p => p.AlternateMailingAddressId);

            HasOptional(p => p.AddressVerificationTypeLookup)
                .WithMany()
                .HasForeignKey(p => p.AddressVerificationTypeLookupId);

            #endregion

            #region Properties

            ToTable("EARequestContactInfo");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CountyOfResidenceId)
                .HasColumnType("int")
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

            Property(p => p.PhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.CanTextPhone)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.AlternatePhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.CanTextAlternatePhone)
                .HasColumnType("bit")
                .IsOptional();
            
            Property(p => p.EmailAddress)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.BestWayToReach)
                .HasColumnType("varchar")
                .HasMaxLength(6)
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
