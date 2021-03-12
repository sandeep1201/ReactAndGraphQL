using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EmploymentInformationConfig : BaseConfig<EmploymentInformation>
    {
        public EmploymentInformationConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.EmploymentInformations)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EmploybilityPlanEmploymentInfoBridges)
                .WithRequired(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.POPClaimEmploymentBridges)
                .WithRequired(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.JobType)
                .WithMany()
                .HasForeignKey(p => p.JobTypeId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.WageHour)
                .WithMany()
                .HasForeignKey(p => p.WageHoursId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.Absences)
                .WithRequired(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EmploymentVerifications)
                .WithRequired(p => p.EmploymentInformation)
                .HasForeignKey(p => p.EmploymentInformationId)
                .WillCascadeOnDelete(false);

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

            Property(p => p.TotalSubsidizedHours)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
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
