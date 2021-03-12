using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ContractAreaConfig : BaseConfig<ContractArea>
    {
        public ContractAreaConfig()
        {
            #region Relationships

            HasOptional(p =>  p.Organization)
                .WithMany(p => p.ContractAreas)
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.EnrolledProgram)
                .WithMany(p => p.ContractAreas)
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("ContractArea");

            Property(p => p.Name)
                .HasColumnName("ContractAreaName")
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsOptional();

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

            Property(p => p.ActivatedDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.InActivatedDate)
                .HasColumnType("date")
                .IsOptional();

            #endregion
        }
    }
}
