using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class DriverLicenseConfig : BaseCommonModelConfig<DriverLicense>
    {
        public DriverLicenseConfig()
        {
            #region Relationships

            HasOptional(p => p.State)
                .WithMany()
                .HasForeignKey(p => p.StateId);

            HasOptional(p => p.DriverLicenseType)
                .WithMany()
                .HasForeignKey(p => p.DriverLicenseTypeId);

            #endregion

            #region Properties

            ToTable("DriverLicense");

            Property(p => p.StateId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DriverLicenseTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ExpiredDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Details)
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
