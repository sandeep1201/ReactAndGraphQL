using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class SchoolCollegeEstablishmentConfig : BaseCommonModelConfig<SchoolCollegeEstablishment>
    {
        public SchoolCollegeEstablishmentConfig()
        {
            #region Relationships

            HasOptional(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId);

            HasMany(p => p.PostSecondaryColleges)
                .WithOptional(p => p.SchoolCollegeEstablishment)
                .HasForeignKey(p => p.SchoolCollegeEstablishmentId);

            HasMany(p => p.EducationSections)
                .WithOptional(p => p.SchoolCollegeEstablishment)
                .HasForeignKey(p => p.SchoolCollegeEstablishmentId);

            #endregion

            #region Properties

            ToTable("SchoolCollegeEstablishment");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.Street)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.CityId)
                .HasColumnType("int")
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
