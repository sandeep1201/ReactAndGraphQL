using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmploymentInformationConfig : BaseCommonModelConfig<EmploymentInformation>
    {
        public EmploymentInformationConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.WorkHistorySection)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.WorkHistorySectionId);

            HasOptional(p => p.JobType)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.JobTypeId);

            HasOptional(p => p.City)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.CityId);

            HasOptional(p => p.Contact)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.ContactId);

            HasOptional(p => p.LeavingReason)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.LeavingReasonId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.DeleteReasonId);

            HasOptional(p => p.OtherJobInformation)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.OtherJobInformationId);

            HasOptional(p => p.WageHour)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.WageHoursId);

            HasOptional(p => p.EmploymentProgramType)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.EmploymentProgramtypeId);

            //HasOptional(p => p.EmployerOfRecordType)
            //    .WithMany(p => p.EmploymentInformations)
            //    .HasForeignKey(p => p.EmployerOfRecordTypeId);

            HasMany(p => p.Absences)
                .WithOptional(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId);

            HasMany(p => p.EmploymentInformationBenefitsOfferedTypeBridges)
                .WithOptional(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId);

            HasMany(p => p.EmploymentInformationJobDutiesDetailsBridges)
                .WithOptional(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId);

            HasMany(p => p.EPEIBridges)
                .WithOptional(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId);

            HasMany(p => p.EmployerOfRecordInformations)
                .WithRequired(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId);

            HasMany(p => p.WeeklyHoursWorkedEntries)
                .WithRequired()
                .HasForeignKey(p => p.EmploymentInformationId);

            HasMany(p => p.EmploymentVerifications)
                .WithRequired(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId);

            #endregion

            #region Properties

            ToTable("EmploymentInformation");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.WorkHistorySectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.JobTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.JobBeginDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.JobEndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.IsCurrentlyEmployed)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.JobPosition)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.CompanyName)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.Fein)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.StreetAddress)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.ZipAddress)
                .HasColumnType("varchar")
                .HasMaxLength(9)
                .IsOptional();

            Property(p => p.CityId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContactId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.JobDutiesId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.LeavingReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.LeavingReasonDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.OtherJobInformationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WageHoursId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.EmploymentProgramtypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EmployerOfRecordTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EmploymentSequenceNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.OriginalOfficeNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.IsCurrentJobAtCreation)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsConverted)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

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
