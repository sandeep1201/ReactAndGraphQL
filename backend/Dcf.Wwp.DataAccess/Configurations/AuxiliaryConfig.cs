using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class AuxiliaryConfig : BaseConfig<Auxiliary>
    {
        public AuxiliaryConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.Auxiliaries)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.Office)
                .WithMany()
                .HasForeignKey(p => p.OfficeId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.CountyAndTribe)
                .WithMany()
                .HasForeignKey(p => p.CountyId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.AuxiliaryReason)
                .WithMany()
                .HasForeignKey(p => p.AuxiliaryReasonId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.ParticipationPeriod)
                .WithMany()
                .HasForeignKey(p => p.ParticipationPeriodId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.AuxiliaryStatuses)
                .WithRequired(p => p.Auxiliary)
                .HasForeignKey(p => p.AuxiliaryId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("Auxiliary");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OfficeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CountyId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.AuxiliaryReasonId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipationPeriodId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CaseNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.ParticipationPeriodYear)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.OriginalPayment)
                .HasColumnType("decimal")
                .HasPrecision(10, 2)
                .IsRequired();

            Property(p => p.RequestedAmount)
                .HasColumnType("decimal")
                .HasPrecision(10, 2)
                .IsRequired();

            Property(p => p.IsSystemRequested)
                .HasColumnType("bit")
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
                .IsRequired();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.AGSequenceNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.RequestedUserForApprovalAndDB2)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            #endregion
        }
    }
}
