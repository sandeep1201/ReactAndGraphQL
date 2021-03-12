using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EnrolledProgramEPActivityTypeBridgeConfig : BaseConfig<EnrolledProgramEPActivityTypeBridge>
    {
        public EnrolledProgramEPActivityTypeBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.ActivityType)
                .WithMany()
                .HasForeignKey(p => p.ActivityTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("EnrolledProgramEPActivityTypeBridge");

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ActivityTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsSelfDirected)
                .HasColumnType("bit")
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

            Property(p => p.IsUpfrontActivity)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsSanctionable)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsAssessmentRelated)
                .HasColumnType("bit")
                .IsOptional();

            #endregion
        }
    }
}
