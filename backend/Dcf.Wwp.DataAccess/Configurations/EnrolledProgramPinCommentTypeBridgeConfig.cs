using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EnrolledProgramPinCommentTypeBridgeConfig : BaseConfig<EnrolledProgramPinCommentTypeBridge>
    {
        public EnrolledProgramPinCommentTypeBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.PinCommentType)
                .WithMany()
                .HasForeignKey(p => p.PinCommentTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("EnrolledProgramPinCommentTypeBridge");

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PinCommentTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.SystemUseOnly)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.EffectiveDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
                .IsOptional();

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
