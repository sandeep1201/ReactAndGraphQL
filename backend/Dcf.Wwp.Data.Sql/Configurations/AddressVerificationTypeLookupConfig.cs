using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class AddressVerificationTypeLookupConfig : BaseCommonModelConfig<AddressVerificationTypeLookup>
    {
        public AddressVerificationTypeLookupConfig()
        {
            #region Relationships

            HasMany(p => p.ParticipantContactInfoes)
                .WithOptional(p => p.AddressVerificationTypeLookup)
                .HasForeignKey(p => p.AddressVerificationTypeLookupId);

            HasMany(p => p.AlternateMailingAddresses)
                .WithOptional(p => p.AddressVerificationTypeLookup)
                .HasForeignKey(p => p.AddressVerificationTypeLookupId);

            #endregion

            #region Properties

            ToTable("AddressVerificationTypeLookup");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
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

            #endregion
        }
    }
}
