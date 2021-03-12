using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class CityConfig : BaseCommonModelConfig<City>
    {
        public CityConfig()
        {
            #region Relationships

            HasOptional(p => p.Country)
                .WithMany(p => p.Cities)
                .HasForeignKey(p => p.CountryId);

            HasOptional(p => p.State)
                .WithMany(p => p.Cities)
                .HasForeignKey(p => p.StateId);

            HasMany(p => p.SchoolCollegeEstablishments)
                .WithOptional(p => p.City)
                .HasForeignKey(p => p.CityId);

            HasMany(p => p.InvolvedWorkPrograms)
                .WithOptional(p => p.City)
                .HasForeignKey(p => p.CityId);

            HasMany(p => p.EmploymentInformations)
                .WithOptional(p => p.City)
                .HasForeignKey(p => p.CityId);

            HasMany(p => p.NonSelfDirectedActivities)
                .WithOptional(p => p.City)
                .HasForeignKey(p => p.CityId);

            HasMany(p => p.EmployerOfRecordInformations)
                .WithOptional(p => p.City)
                .HasForeignKey(p => p.CityId);

            HasMany(p => p.ParticipantContactInfoes)
                .WithOptional(p => p.City)
                .HasForeignKey(p => p.CityAddressId);

            HasMany(p => p.AlternateMailingAddresses)
                .WithOptional(p => p.City)
                .HasForeignKey(p => p.CityAddressId);

            #endregion

            #region Properties

            ToTable("City");

            Property(p => p.Name)
                .HasColumnType("nvarchar")
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

            Property(p => p.LatitudeNumber)
                .HasColumnName("LatitudeNumber")
                .HasColumnType("decimal")
                .HasPrecision(12, 9)
                .IsOptional();

            Property(p => p.LongitudeNumber)
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
