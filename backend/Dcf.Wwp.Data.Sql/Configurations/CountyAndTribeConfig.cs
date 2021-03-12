using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class CountyAndTribeConfig : BaseConfig<CountyAndTribe>
    {
        public CountyAndTribeConfig()
        {
            #region Relationships

            HasMany(p => p.Offices)
                .WithOptional(p => p.CountyAndTribe)
                .HasForeignKey(p => p.CountyandTribeId);

            #endregion

            #region Properties

            ToTable("CountyAndTribe");

            Property(p => p.CountyNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.CountyName)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.IsCounty)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.AgencyName)
                .HasColumnType("varchar")
                .HasMaxLength(40)
                .IsOptional();

            Property(p => p.LocationNumber)
                .HasColumnType("smallint")
                .IsOptional();

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
