using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EligibilityByFPLConfig : BaseCommonModelConfig<EligibilityByFPL>
    {
        public EligibilityByFPLConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("EligibilityByFPL");

            Property(p => p.GroupSize)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Pct150PerMonth)
                .HasColumnType("decimal")
                .HasPrecision(9, 2)
                .IsOptional();

            Property(p => p.Pct115PerMonth)
                .HasColumnType("decimal")
                .HasPrecision(9, 2)
                .IsOptional();

            Property(p => p.EffectiveDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
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
                .IsRequired();

            #endregion
        }
    }
}
