using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EnrolledProgramActivityCompletionReasonBridgeConfig : BaseConfig<EnrolledProgramActivityCompletionReasonBridge>
    {
        public EnrolledProgramActivityCompletionReasonBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.ActivityCompletionReason)
                .WithMany()
                .HasForeignKey(p => p.ActivityCompletionReasonId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("EnrolledProgramActivityCompletionReasonBridge");

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActivityCompletionReasonId)
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
