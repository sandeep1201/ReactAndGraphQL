using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class FeatureURLConfig : BaseCommonModelConfig<FeatureURL>
    {
        public FeatureURLConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("FeatureURL");

            Property(p => p.Feature)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.URL)
                .HasColumnType("varchar(max)")
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
