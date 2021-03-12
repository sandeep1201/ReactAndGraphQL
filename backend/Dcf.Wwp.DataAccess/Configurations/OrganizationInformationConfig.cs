using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class OrganizationInformationConfig : BaseConfig<OrganizationInformation>
    {
        public OrganizationInformationConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.OrganizationLocations)
                .WithRequired(p => p.OrganizationInformation)
                .HasForeignKey(p => p.OrganizationInformationId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("OrganizationInformation");

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsRequired();

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
