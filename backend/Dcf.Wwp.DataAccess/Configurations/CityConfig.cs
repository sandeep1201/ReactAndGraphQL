using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class CityConfig : BaseConfig<City>
    {
        public CityConfig()
        {
            #region Relationships

            HasOptional(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.State)
                .WithMany(p => p.Cities)
                .HasForeignKey(p => p.StateId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("City");

            HasKey(p => p.Id);

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.GooglePlaceId)
                .HasColumnType("varchar")
                .HasMaxLength(1024)
                .IsOptional();

            Property(p => p.CountryId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StateId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Latitude)
                .HasColumnName("LatitudeNumber")
                .HasColumnType("decimal")
                .HasPrecision(12,9)
                .IsOptional();

            Property(p => p.Longitude)
                .HasColumnName("LongitudeNumber")
                .HasColumnType("decimal")
                .HasPrecision(12, 9)
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
