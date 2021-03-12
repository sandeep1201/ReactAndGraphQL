using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class CDOTrackingConfig : BaseConfig<CDOTracking>
    {
        public CDOTrackingConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("CDOTracking");

            Property(p => p.WUID)
                .HasColumnType("varchar")
                .HasMaxLength(25)
                .IsOptional();

            Property(p => p.StartTimestamp)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EndTimestamp)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.CDODate)
                .HasColumnType("date")
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
