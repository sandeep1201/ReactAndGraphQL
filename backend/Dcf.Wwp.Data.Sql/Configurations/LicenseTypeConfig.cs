using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class LicenseTypeConfig : BaseCommonModelConfig<LicenseType>
    {
        public LicenseTypeConfig()
        {
            #region Relationships

            HasMany(p => p.PostSecondaryLicenses)
                .WithOptional(p => p.LicenseType)
                .HasForeignKey(p => p.LicenseTypeId);

            #endregion

            #region Properties

            ToTable("LicenseType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
