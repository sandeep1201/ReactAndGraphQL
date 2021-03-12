using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EARequestStatusConfig : BaseConfig<EARequestStatus>
    {
        public EARequestStatusConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaRequestStatuses)
                .HasForeignKey(p => p.RequestId);

            HasRequired(p => p.EaStatus)
                .WithMany()
                .HasForeignKey(p => p.StatusId);

            HasMany(p => p.EaRequestStatusReasons)
                .WithRequired(p => p.EaRequestStatus)
                .HasForeignKey(p => p.RequestStatusId);

            #endregion

            #region Properties

            ToTable("EARequestStatus");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.StatusId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.StatusDeadLineDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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
