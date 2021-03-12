using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WorkHistorySectionConfig : BaseCommonModelConfig<WorkHistorySection>
    {
        public WorkHistorySectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.EmploymentStatusType)
                .WithMany()
                .HasForeignKey(p => p.EmploymentStatusTypeId);

            HasMany(p => p.EmploymentInformations)
                .WithOptional(p => p.WorkHistorySection)
                .HasForeignKey(p => p.WorkHistorySectionId);

            HasMany(p => p.WorkHistorySectionEmploymentPreventionTypeBridges)
                .WithRequired(p => p.WorkHistorySection)
                .HasForeignKey(p => p.WorkHistorySectionId);

            HasOptional(p => p.YesNoUnknownLookup)
                .WithMany()
                .HasForeignKey(p => p.HasCareerAssessment);

            #endregion

            #region Properties

            ToTable("WorkHistorySection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmploymentStatusTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasVolunteered)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.NonFullTimeDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.PreventionFactors)
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

            Property(p => p.HasCareerAssessment)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HasCareerAssessmentNotes)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            #endregion
        }
    }
}
