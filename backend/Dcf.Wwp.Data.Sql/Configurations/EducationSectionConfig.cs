using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EducationSectionConfig : BaseCommonModelConfig<EducationSection>
    {
        public EducationSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.EducationSections)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.SchoolGraduationStatus)
                .WithMany(p => p.EducationSections)
                .HasForeignKey(p => p.SchoolGraduationStatusId);

            HasOptional(p => p.SchoolCollegeEstablishment)
                .WithMany(p => p.EducationSections)
                .HasForeignKey(p => p.SchoolCollegeEstablishmentId);

            HasOptional(p => p.SchoolGradeLevel)
                .WithMany(p => p.EducationSections)
                .HasForeignKey(p => p.LastGradeLevelCompletedId);

            HasOptional(p => p.CertificateIssuingAuthority)
                .WithMany()
                .HasForeignKey(p => p.CertificateIssuingAuthorityId);

            #endregion

            #region Properties

            ToTable("EducationSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.SchoolGraduationStatusId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SchoolCollegeEstablishmentId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.LastGradeLevelCompletedId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CertificateIssuingAuthorityId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CertificateYearAwarded)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasEverAttendedSchool)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsCurrentlyEnrolled)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsWorkingOnCertificate)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.LastYearAttended)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasEducationPlan)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.EducationPlanDetails)
                .HasColumnType("varchar")
                .HasMaxLength(450)
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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
