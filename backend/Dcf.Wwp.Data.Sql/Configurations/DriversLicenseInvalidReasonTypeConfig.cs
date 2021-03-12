using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class DriversLicenseInvalidReasonTypeConfig : BaseCommonModelConfig<DriversLicenseInvalidReasonType>
    {
        public DriversLicenseInvalidReasonTypeConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("DriversLicenseInvalidReasonType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
                .IsOptional();

            #endregion
        }
    }
}
