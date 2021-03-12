using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class PostSecondaryEducationSectionConfig : BaseConfig<PostSecondaryEducationSection>
    {
        public PostSecondaryEducationSectionConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.PostSecondaryEducationSections)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.PostSecondaryColleges)
                .WithRequired(p => p.PostSecondaryEducationSection)
                .HasForeignKey(p => p.PostSecondaryEducationSectionId);

            HasMany(p => p.PostSecondaryDegrees)
                .WithRequired(p => p.PostSecondaryEducationSection)
                .HasForeignKey(p => p.PostSecondaryEducationSectionId);

            HasMany(p => p.PostSecondaryLicenses)
                .WithRequired(p => p.PostSecondaryEducationSection)
                .HasForeignKey(p => p.PostSecondaryEducationSectionId);

            #endregion

            #region Properties

            ToTable("PostSecondaryEducationSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DidAttendCollege)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsWorkingOnLicensesOrCertificates)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DoesHaveDegrees)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

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
