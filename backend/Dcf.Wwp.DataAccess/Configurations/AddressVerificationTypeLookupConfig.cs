using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class AddressVerificationTypeLookupConfig : BaseConfig<AddressVerificationTypeLookup>
    {
        public AddressVerificationTypeLookupConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            ToTable("AddressVerificationTypeLookupConfig");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

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
