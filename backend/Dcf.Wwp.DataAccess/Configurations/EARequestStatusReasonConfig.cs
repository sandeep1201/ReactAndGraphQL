using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EARequestStatusReasonConfig : BaseConfig<EARequestStatusReason>
    {
        public EARequestStatusReasonConfig()
        {
            #region Relationship

            HasRequired(p => p.EaRequestStatus)
                .WithMany(p => p.EaRequestStatusReasons)
                .HasForeignKey(p => p.RequestStatusId);

            HasRequired(p => p.StatusReason)
                .WithMany()
                .HasForeignKey(p => p.StatusReasonId);

            #endregion

            #region Properties

            ToTable("EARequestStatusReason");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.RequestStatusId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.StatusReasonId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .IsRequired();

            #endregion
        }
    }
}
