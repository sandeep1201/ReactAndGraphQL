using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class CountryConfig : BaseCommonModelConfig<Country>
    {
        public CountryConfig()
        {
            #region Relationships

            HasMany(p => p.Cities)
                .WithOptional(p => p.Country)
                .HasForeignKey(p => p.CountryId);

            HasMany(p => p.States)
                .WithOptional(p => p.Country)
                .HasForeignKey(p => p.CountryId);

            #endregion

            #region Properties

            ToTable("Country");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(2)
                .IsOptional();

            Property(p => p.IsNonStandard)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
