using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ParticipantConfig : BaseConfig<Participant>
    {
        public ParticipantConfig()
        {
            #region Relationships

            // see comments in Participart.cs nav props

            // '.WithOne(p => <lambda>)' is an EF Core semantic,
            // In EF6 it's either
            // .WithOptional(p => <lambda>) or WithRequired(p => <lambda>)

            HasMany(p => p.ActionsNeeded)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.AKAs)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.BarrierDetails)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.BarrierSections)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.ChildYouthSections)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.ConfidentialPinInformations)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.Contacts)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.EducationExams)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.EducationSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.EmploymentInformations)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.EmployabilityPlans)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.FamilyBarriersSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.HousingSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.InformalAssessments)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.LanguageSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.LegalIssuesSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.MilitaryTrainingSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.NonCustodialParentsReferralSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.NonCustodialParentsSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.OfficeTransfers)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.OtherDemographics)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.ParticipantChildRelationshipBridges)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.ParticipantContactInfoes)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.ParticipantEnrolledPrograms)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.PostSecondaryEducationSections)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.RecentParticipants)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.RequestsForAssistance)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.TimeLimits)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantID);

            HasMany(p => p.TimeLimitExtensions)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.TimeLimitSummaries)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.TransportationSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.WorkHistorySections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.WorkProgramSections)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.WorkerParticipantBridges)
                .WithOptional(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.ParticipantEnrolledProgramCutOverBridges)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.Transactions)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.WorkerTaskLists)
                .WithRequired(p => p.Participant)
                .HasForeignKey(p => p.ParticipantId);

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

            Property(p => p.MiddleInitialName)
                .HasColumnType("varchar")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.SuffixName)
                .HasColumnType("varchar")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.DateOfBirth)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.DateOfDeath)
                .HasColumnType("date")
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

            Property(p => p.MCI_ID)
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

            Property(p => p.TotalLifetimeHoursDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.HasBeenThroughClientReg)
                .HasColumnType("bit")
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

            Property(p => p.Is60DaysVerified)
                .HasColumnType("bit")
                .IsOptional();

            #endregion
        }
    }
}
