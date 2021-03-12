using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class InformalAssessmentConfig : BaseConfig<InformalAssessment>
    {
        public InformalAssessmentConfig()
        {
            #region Relationships

            HasOptional(p => p.AssessmentType)
                .WithMany()
                .HasForeignKey(p => p.AssessmentTypeId);

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.LanguageAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.LanguageAssessmentSectionId);

            HasOptional(p => p.WorkHistoryAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.WorkHistoryAssessmentSectionId);

            HasOptional(p => p.WorkProgramAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.WorkProgramAssessmentSectionId);

            HasOptional(p => p.PostSecondaryEducationAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.PostSecondaryEducationAssessmentSectionId);

            HasOptional(p => p.MilitaryTrainingAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.MilitaryTrainingAssessmentSectionId);

            HasOptional(p => p.HousingAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.HousingAssessmentSectionId);

            HasOptional(p => p.TransportationAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.TransportationAssessmentSectionId);

            HasOptional(p => p.LegalIssuesAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.LegalIssuesAssessmentSectionId);

            HasOptional(p => p.BarrierAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.BarriersAssessmentSectionId);

            HasOptional(p => p.ChildYouthSupportsAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.ChildYouthSupportsAssessmentSectionId);

            HasOptional(p => p.FamilyBarriersAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.FamilyBarriersAssessmentSectionId);

            HasOptional(p => p.NonCustodialParentsAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.NonCustodialParentsAssessmentSectionId);

            HasOptional(p => p.NonCustodialParentsReferralAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.NonCustodialParentsReferralAssessmentSectionId);

            HasOptional(p => p.EducationAssessmentSection)
                .WithMany(p => p.InformalAssessments)
                .HasForeignKey(p => p.EducationAssessmentSectionId);

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
