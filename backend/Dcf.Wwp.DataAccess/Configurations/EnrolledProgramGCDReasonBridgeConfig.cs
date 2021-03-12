using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EnrolledProgramGCDReasonBridgeConfig : BaseConfig<EnrolledProgramGCDReasonBridge>
    {
        public EnrolledProgramGCDReasonBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.GoodCauseDeniedReason)
                .WithMany()
                .HasForeignKey(p => p.GoodCauseDeniedReasonId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("EnrolledProgramGCDReasonBridge");

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.GoodCauseDeniedReasonId)
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
