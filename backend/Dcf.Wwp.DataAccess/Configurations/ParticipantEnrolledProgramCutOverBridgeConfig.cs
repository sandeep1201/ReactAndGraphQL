using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ParticipantEnrolledProgramCutOverBridgeConfig : BaseConfig<ParticipantEnrolledProgramCutOverBridge>
    {
        public ParticipantEnrolledProgramCutOverBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.ParticipantEnrolledProgramCutOverBridges)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId);

            #endregion

            #region Properties

            ToTable("ParticipantEnrolledProgramCutOverBridge");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CutOverDate)
                .HasColumnType("date")
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

            #endregion
        }
    }
}
