using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class DriverLicenseTypeConfig : BaseCommonModelConfig<DriverLicenseType>
    {
        public DriverLicenseTypeConfig()
        {
            #region Relationships

            HasMany(p => p.DriverLicenses)
                .WithOptional(p => p.DriverLicenseType)
                .HasForeignKey(p => p.DriverLicenseTypeId);

            #endregion

            #region Properties

            ToTable("DriverLicenseType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
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
                .IsOptional();

            #endregion
        }
    }
}
