using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class PostSecondaryCollegeConfig : BaseCommonModelConfig<PostSecondaryCollege>
    {
        public PostSecondaryCollegeConfig()
        {
            #region Relationships

            HasRequired(p => p.PostSecondaryEducationSection)
                .WithMany(p => p.PostSecondaryColleges)
                .HasForeignKey(p => p.PostSecondaryEducationSectionId);

            HasOptional(p => p.SchoolCollegeEstablishment)
                .WithMany(p => p.PostSecondaryColleges)
                .HasForeignKey(p => p.SchoolCollegeEstablishmentId);

            #endregion

            #region Properties

            ToTable("PostSecondaryCollege");

            Property(p => p.PostSecondaryEducationSectionId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.SchoolCollegeEstablishmentId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasGraduated)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.LastYearAttended)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CurrentlyAttending)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.Semesters)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Credits)
                .HasColumnType("decimal")
                .HasPrecision(18, 10)
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.OriginId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SortOrder)
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

            //Ignore(p => p.IsCurrentlyAttending);

            #endregion
        }
    }
}
