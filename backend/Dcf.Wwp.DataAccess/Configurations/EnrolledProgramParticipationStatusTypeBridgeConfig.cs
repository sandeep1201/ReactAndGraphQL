using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EnrolledProgramParticipationStatusTypeBridgeConfig : BaseConfig<EnrolledProgramParticipationStatusTypeBridge>
    {
        public EnrolledProgramParticipationStatusTypeBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.ParticipationStatusType)
                .WithMany()
                .HasForeignKey(p => p.ParticipationStatusTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("EnrolledProgramParticipationStatusTypeBridge");

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipationStatusTypeId)
                .HasColumnType("int")
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
