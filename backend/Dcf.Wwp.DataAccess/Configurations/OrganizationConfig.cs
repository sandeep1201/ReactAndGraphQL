using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class OrganizationConfig : BaseConfig<Organization>
    {
        public OrganizationConfig()
        {
            #region Relationships

            HasMany(p => p.Workers)
                .WithOptional(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ContractAreas)
                .WithOptional(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.POPClaims)
                .WithRequired(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.POPClaimHighWages)
                .WithRequired(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("Organization");

            Property(p => p.EntsecAgencyCode)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsOptional();

            Property(p => p.AgencyName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.DB2AgencyName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ActivatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.InActivatedDate)
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
                .IsOptional();

            #endregion
        }
    }
}
