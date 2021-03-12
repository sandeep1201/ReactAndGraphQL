using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TimeLimitExtensionConfig : BaseCommonModelConfig<TimeLimitExtension>
    {
        public TimeLimitExtensionConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.TimeLimitExtensions)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.ExtensionDecision)
                .WithMany()
                .HasForeignKey(p => p.ExtensionDecisionId);

            HasOptional(p => p.TimeLimitType)
                .WithMany()
                .HasForeignKey(p => p.TimeLimitTypeId);

            HasOptional(p => p.ApprovalReason)
                .WithMany()
                .HasForeignKey(p => p.ApprovalReasonId);

            HasOptional(p => p.DenialReason)
                .WithMany(p => p.TimeLimitExtensions)
                .HasForeignKey(p => p.DenialReasonId);

            HasOptional(p => p.DeleteReason)
                .WithMany()
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("TimeLimitExtension");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PinNum)
                .HasColumnName("PIN_NUM")
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.ExtensionDecisionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TimeLimitTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DecisionDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.InitialDiscussionDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.ApprovalReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.IsPendingDVR)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsReceivingDVR)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsPendingSSIorSSDI)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.BeginMonth)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EndMonth)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.ExtensionSequence)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsBackDatedExtenstion)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
