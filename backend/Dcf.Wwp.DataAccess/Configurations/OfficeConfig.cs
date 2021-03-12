using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class OfficeConfig : BaseConfig<Office>
    {
        public OfficeConfig()
        {
            #region Relationships

            HasRequired(p => p.ContractArea)
                .WithMany()
                .HasForeignKey(p => p.ContractAreaId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.CountyAndTribe)
                .WithMany()
                .HasForeignKey(p => p.CountyandTribeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("WWPOffice");

            Property(p => p.OfficeNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.OfficeName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.MFWPOfficeNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.MFEligibilityOfficeNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.CountyandTribeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContractAreaId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.MFLocationNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.ActiviatedDate)
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
