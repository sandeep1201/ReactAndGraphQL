namespace DCF.Core.Domain
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TimelimitsDataContext : DbContext
    {
        public TimelimitsDataContext()
            : base("name=TimelimitsDataContext")
        {
        }

        public virtual DbSet<ApprovalReason> ApprovalReasons { get; set; }
        public virtual DbSet<AuxiliaryPayment> AuxiliaryPayments { get; set; }
        public virtual DbSet<ChangeReason> ChangeReasons { get; set; }
        public virtual DbSet<DeleteReason> DeleteReasons { get; set; }
        public virtual DbSet<DenialReason> DenialReasons { get; set; }
        public virtual DbSet<EnrolledProgram> EnrolledPrograms { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<ParticipantEnrolledProgram> ParticipantEnrolledPrograms { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<TimeLimit> TimeLimits { get; set; }
        public virtual DbSet<TimeLimitExtension> TimeLimitExtensions { get; set; }
        public virtual DbSet<TimeLimitState> TimeLimitStates { get; set; }
        public virtual DbSet<TimeLimitSummary> TimeLimitSummaries { get; set; }
        public virtual DbSet<TimeLimitType> TimeLimitTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalReason>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ApprovalReason>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ApprovalReason>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<AuxiliaryPayment>()
                .Property(e => e.PinNumber)
                .HasPrecision(10, 0);

            modelBuilder.Entity<AuxiliaryPayment>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AuxiliaryPayment>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<ChangeReason>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ChangeReason>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ChangeReason>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<DeleteReason>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<DeleteReason>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<DeleteReason>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<DenialReason>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<DenialReason>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<DenialReason>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<EnrolledProgram>()
                .Property(e => e.ProgramName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EnrolledProgram>()
                .Property(e => e.SubprogramName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EnrolledProgram>()
                .Property(e => e.ProgramType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EnrolledProgram>()
                .Property(e => e.DescriptionText)
                .IsUnicode(false);

            modelBuilder.Entity<EnrolledProgram>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.PinNumber)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Participant>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.MiddleInitialName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.SuffixName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.GenderIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.AliasResponse)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.BirthVerificationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.BirthPlaceCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.CitizenshipVerificationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.DCLCitizenshipSwitch)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.DeathVerificationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.LanguageCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.PrimarySSNNumber)
                .HasPrecision(9, 0);

            modelBuilder.Entity<Participant>()
                .Property(e => e.PseudoSSNNumber)
                .HasPrecision(9, 0);

            modelBuilder.Entity<Participant>()
                .Property(e => e.RaceCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.SSNAppointmentVerificationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.SSNValidatedCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.USCitizenSwitch)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.WorkerAlert1Code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.WorkerAlert2Code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.MaidNumber)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Participant>()
                .Property(e => e.ChildElsewhereSwitch)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.ChildVerificationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.AmericanIndianIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.AsianIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.BlackIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.HispanicIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.PacificIslanderIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.WhiteIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.MCI_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Participant>()
                .Property(e => e.MACitizenVerificationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.TribeChildMemberIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.TribeChildVerificationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.TribalMemberIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.TribalMemberVerificationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.DeathDateSourceCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.WorkerOverideVerificationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.ConversionProjectDetails)
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Participant>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Participant>()
                .HasMany(e => e.ParticipantEnrolledPrograms)
                .WithRequired(e => e.Participant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.RFANumber)
                .HasPrecision(10, 0);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.CurrentRegCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.CompletionReasonCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.AuditIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.CaseManagerId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.CASENumber)
                .HasPrecision(10, 0);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.SpecialCircumstancesCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.ReferralRegistrationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.StatusCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.CensusTractNumber)
                .HasPrecision(6, 2);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.LatitudeNumber)
                .HasPrecision(9, 6);

            modelBuilder.Entity<ParticipantEnrolledProgram>()
                .Property(e => e.LongitudeNumber)
                .HasPrecision(9, 6);

            modelBuilder.Entity<State>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<TimeLimit>()
                .Property(e => e.ChangeReasonDetails)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimit>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimit>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimit>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<TimeLimit>()
                .Property(e => e.PIN_NUM)
                .HasPrecision(10, 0);

            modelBuilder.Entity<TimeLimitExtension>()
                .Property(e => e.Details)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimitExtension>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimitExtension>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimitExtension>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<TimeLimitExtension>()
                .Property(e => e.PIN_NUM)
                .HasPrecision(10, 0);

            modelBuilder.Entity<TimeLimitState>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimitState>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimitState>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimitState>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<TimeLimitState>()
                .HasMany(e => e.TimeLimits)
                .WithOptional(e => e.TimeLimitState)
                .HasForeignKey(e => e.StateId);

            modelBuilder.Entity<TimeLimitSummary>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimitSummary>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<TimeLimitType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimitType>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLimitType>()
                .Property(e => e.RowVersion)
                .IsFixedLength();
        }
    }
}
