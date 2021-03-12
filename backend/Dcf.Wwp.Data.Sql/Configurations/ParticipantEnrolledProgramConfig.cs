using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ParticipantEnrolledProgramConfig : BaseCommonModelConfig<ParticipantEnrolledProgram>
    {
        public ParticipantEnrolledProgramConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId);

            HasOptional(p => p.EnrolledProgramStatusCode)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramStatusCodeId);

            HasOptional(p => p.Worker)
                .WithMany(p => p.ParticipantEnrolledPrograms)
                .HasForeignKey(p => p.WorkerId);

            HasOptional(p => p.CompletionReason)
                .WithMany()
                .HasForeignKey(p => p.CompletionReasonId);

            HasOptional(p => p.RequestForAssistance)
                .WithMany(p => p.ParticipantEnrolledPrograms)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasOptional(p => p.Office)
                .WithMany(p => p.ParticipantEnrolledPrograms)
                .HasForeignKey(p => p.OfficeId);

            HasOptional(p => p.LFFEP)
                .WithMany(p => p.ParticipantEnrolledPrograms1)
                .HasForeignKey(p => p.LFFEPId);

            HasMany(p => p.OfficeTransfers)
                .WithRequired(p => p.ParticipantEnrolledProgram)
                .HasForeignKey(p => p.ParticipantEnrolledProgramId);

            HasMany(p => p.PEPOtherInformations)
                .WithOptional(p => p.ParticipantEnrolledProgram)
                .HasForeignKey(p => p.PEPId);

            HasMany(p => p.EmployabilityPlans)
                .WithRequired(p => p.ParticipantEnrolledProgram)
                .HasForeignKey(p => p.ParticipantEnrolledProgramId);

            #endregion

            #region Properties

            ToTable("ParticipantEnrolledProgram");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EnrolledProgramStatusCodeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ReferralDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.EnrollmentDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.DisenrollmentDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.CASENumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.ReferralRegistrationCode)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.CurrentRegCode)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.AGSequenceNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.CaseManagerId)
                .HasColumnType("char")
                .HasMaxLength(6)
                .IsOptional();

            Property(p => p.WorkerId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CompletionReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.RequestForAssistanceId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OfficeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.LFFEPId)
                .HasColumnType("int")
                .IsOptional();

            #region Ignored Properties

            Ignore(p => p.IsDisenrolled);
            Ignore(p => p.IsEnrolled);
            Ignore(p => p.IsReferred);
            Ignore(p => p.IsW2);
            Ignore(p => p.IsTmj);
            Ignore(p => p.IsTJ);
            Ignore(p => p.IsCF);
            Ignore(p => p.IsFCDP);
            Ignore(p => p.IsLF);

            #endregion

            #endregion

            #region Ignored Properties

            #endregion
        }
    }
}
