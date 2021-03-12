using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ParticipantEnrolledProgramConfig : BaseConfig<ParticipantEnrolledProgram>
    {
        public ParticipantEnrolledProgramConfig()
        {
            #region Relationships

            HasOptional(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Participant)
                .WithMany(p => p.ParticipantEnrolledPrograms)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.Worker)
                .WithMany()
                .HasForeignKey(p => p.WorkerId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.LFFEP)
                .WithMany()
                .HasForeignKey(p => p.LFFEPId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.Office)
                .WithMany()
                .HasForeignKey(p => p.OfficeId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.EnrolledProgramStatusCode)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramStatusCodeId)
                .WillCascadeOnDelete(false);

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
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EnrollmentDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.DisenrollmentDate)
                .HasColumnType("datetime")
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

            Property(p => p.OfficeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.RequestForAssistanceId)
                .HasColumnType("int")
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

            Property(p => p.LFFEPId)
                .HasColumnType("int")
                .IsOptional();

            #endregion
        }
    }
}
