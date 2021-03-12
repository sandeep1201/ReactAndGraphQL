using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class OfficeTransferConfig : BaseCommonModelConfig<OfficeTransfer>
    {
        public OfficeTransferConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.ParticipantEnrolledProgram)
                .WithMany(p => p.OfficeTransfers)
                .HasForeignKey(p => p.ParticipantEnrolledProgramId);

            HasOptional(p => p.SourceOffice)
                .WithMany(p => p.OfficeTransfers)
                .HasForeignKey(p => p.SourceOfficeId);

            HasOptional(p => p.SourceAssignedWorker)
                .WithMany()
                .HasForeignKey(p => p.SourceAssignedWorkerId);

            HasOptional(p => p.DestinationOffice)
                .WithMany(p => p.OfficeTransfers1)
                .HasForeignKey(p => p.DestinationOfficeId);

            HasOptional(p => p.DestinationAssignedWorker)
                .WithMany()
                .HasForeignKey(p => p.DestinationAssignedWorkerId);

            #endregion

            #region Properties

            ToTable("OfficeTransfer");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipantEnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.SourceOfficeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SourceAssignedWorkerId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DestinationOfficeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DestinationAssignedWorkerId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TransferDate)
                .HasColumnType("datetime")
                .IsRequired();

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

            #endregion
        }
    }
}
