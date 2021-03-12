using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ParticipantConfig : BaseConfig<Participant>
    {
        public ParticipantConfig()
        {
            #region Relationships

            HasMany(p => p.Auxiliaries)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.CFParticipationEntries)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.Contacts)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EmployabilityPlans)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ParticipationStatuses)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ParticipantEnrolledPrograms)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ParticipationEntries)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ParParticipationPeriodSummaries)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EARequestParticipantBridges)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.OverPayments)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.DrugScreenings)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ParticipantPlacements)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EaIpvs)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.Plans)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ParticipantEnrolledProgramCutOverBridges)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.POPClaims)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.Transactions)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.CareerAssessments)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.JobReadinesses)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EducationExams)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.WorkerTaskLists)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.InformalAssessments)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            //HasMany(p => p.ActionsNeeded)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.BarrierDetails)
            //    .WithOptional(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.BarrierSections)
            //    .WithOptional(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.ChildYouthSections)
            //    .WithOptional(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.EducationSections)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.EmploymentInformation)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.FamilyBarriersSections)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.HousingSections)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.LanguageSections)
            //    .WithOptional(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.MilitaryTrainingSections)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.NonCustodialParentsReferralSections)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.NonCustodialParentsSections)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.OfficeTransfers)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.PostSecondaryEducationSections)
            //    .WithOptional(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.RequestsForAssistance)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.TimeLimits)
            //    .WithOptional(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            ////TODO: check this // let EF handle this screwy relationship. It knows how to map it
            ////HasMany(p => p.TimelimitClosureLogs)
            ////    .WithOptional(p => p.Participant)
            ////    .HasForeignKey(p => p.ParticipantId)
            ////    .WillCascadeOnDelete(false);

            //HasMany(p => p.TimeLimitExtensions)
            //    .WithOptional(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.TimeLimitSummaries)
            //    .WithOptional(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.TransportationSections)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.WorkerParticipantBridges)
            //    .WithOptional(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.WorkProgramSections)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.WorkHistorySections)
            //    .WithRequired(p => p.Participant)
            //    .HasForeignKey(p => p.ParticipantId)
            //    .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("Participant");

            Property(p => p.PinNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.MiddleInitial)
                .HasColumnName("MiddleInitialName")
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.SuffixName)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.DateOfBirth)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.DateOfDeath)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.GenderIndicator)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.AliasResponse)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.LanguageCode)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.MaxHistorySequenceNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.RaceCode)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.USCitizenSwitch)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.AmericanIndianIndicator)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.AsianIndicator)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.BlackIndicator)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.HispanicIndicator)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.PacificIslanderIndicator)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.WhiteIndicator)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.MciId)
                .HasColumnName("MCI_ID")
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.TribalMemberIndicator)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.TimeLimitStatus)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ConversionProjectDetails)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ConversionDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.HasBeenThroughClientReg)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.TotalLifetimeHoursDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.Is60DaysVerified)
                .HasColumnType("bit")
                .IsOptional();

            #endregion
        }
    }
}
