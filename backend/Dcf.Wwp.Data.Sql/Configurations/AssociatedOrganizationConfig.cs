using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class AssociatedOrganizationConfig : BaseConfig<AssociatedOrganization>
    {
        public AssociatedOrganizationConfig()
        {
            #region Relationships

            HasRequired(p => p.ContractArea)
                .WithMany(p => p.AssociatedOrganizations)
                .HasForeignKey(d => d.ContractAreaId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("AssociatedOrganization");

            Property(p => p.ContractAreaId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActivatedDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.InactivatedDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
