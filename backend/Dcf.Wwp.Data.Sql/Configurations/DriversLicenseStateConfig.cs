using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class DriversLicenseStateConfig : BaseConfig<DriversLicenseState>
    {
        public DriversLicenseStateConfig()
        {
            #region Relationships

            HasRequired(p => p.State)
                .WithMany()
                .HasForeignKey(p => p.StateId);

            #endregion

            #region Properties

            ToTable("DriversLicenseState");

            Property(p => p.StateId)
                .HasColumnType("int")
                .IsRequired();

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
                .IsRequired();

            #endregion
        }
    }
}
