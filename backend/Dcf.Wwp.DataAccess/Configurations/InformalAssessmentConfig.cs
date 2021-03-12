using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class InformalAssessmentConfig : BaseConfig<InformalAssessment>
    {
        public InformalAssessmentConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.ParticipantId);

            #endregion

            #region Properties

            ToTable("InformalAssessment");

            Property(p => p.AssessmentTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.LanguageAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WorkHistoryAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WorkProgramAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PostSecondaryEducationAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.MilitaryTrainingAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HousingAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TransportationAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.LegalIssuesAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BarriersAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ChildYouthSupportsAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.FamilyBarriersAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.NonCustodialParentsAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.NonCustodialParentsReferralAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WorkHistorySectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EducationAssessmentSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
                .IsOptional();

            #endregion
        }
    }
}
