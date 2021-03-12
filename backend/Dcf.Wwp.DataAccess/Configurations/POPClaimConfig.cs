using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class POPClaimConfig : BaseConfig<POPClaim>
    {
        public POPClaimConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.POPClaims)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.Organization)
                .WithMany(p => p.POPClaims)
                .HasForeignKey(p => p.OrganizationId);

            HasRequired(p => p.POPClaimType)
                .WithMany()
                .HasForeignKey(p => p.POPClaimTypeId);

            HasMany(p => p.POPClaimEmploymentBridges)
                .WithRequired(p => p.POPClaim)
                .HasForeignKey(p => p.POPClaimId);

            HasMany(p => p.POPClaimActivityBridges)
                .WithRequired(p => p.POPClaim)
                .HasForeignKey(p => p.POPClaimId);

            #endregion

            #region Properties

            ToTable("POPClaim");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.POPClaimTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ClaimPeriodBeginDate)
                .HasColumnType("date")
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

            Property(p => p.ClaimEffectiveDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.IsProcessed)
                .HasColumnType("bit")
                .IsOptional();

            #endregion
        }
    }
}
