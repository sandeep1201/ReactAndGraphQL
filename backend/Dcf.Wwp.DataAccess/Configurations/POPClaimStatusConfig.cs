using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class POPClaimStatusConfig : BaseConfig<POPClaimStatus>
    {
        public POPClaimStatusConfig()
        {
            #region Relationships

            HasRequired(p => p.POPClaim)
                .WithMany(p => p.POPClaimStatuses)
                .HasForeignKey(p => p.POPClaimId);

            HasRequired(p => p.POPClaimStatusType)
                .WithMany()
                .HasForeignKey(p => p.POPClaimStatusTypeId);

            #endregion

            #region Properties

            ToTable("POPClaimStatus");

            Property(p => p.POPClaimId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.POPClaimStatusTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.POPClaimStatusDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(380)
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
