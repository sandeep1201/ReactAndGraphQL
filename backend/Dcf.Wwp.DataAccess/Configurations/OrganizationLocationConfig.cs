using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class OrganizationLocationConfig : BaseConfig<OrganizationLocation>
    {
        public OrganizationLocationConfig()
        {
            #region Relationships

            HasRequired(p => p.OrganizationInformation)
                .WithMany()
                .HasForeignKey(p => p.OrganizationInformationId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.AddressVerificationTypeLookup)
                .WithMany()
                .HasForeignKey(p => p.AddressVerificationTypeLookupId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("OrganizationLocation");

            Property(p => p.OrganizationInformationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.AddressLine1)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsRequired();

            Property(p => p.CityId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ZipCode)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.AddressVerificationTypeLookupId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EffectiveDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
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
