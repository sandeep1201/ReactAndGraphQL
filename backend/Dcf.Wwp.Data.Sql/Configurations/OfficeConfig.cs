using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class OfficeConfig : BaseConfig<Office>
    {
        public OfficeConfig()
        {
            #region Relationships

            HasOptional(p => p.CountyAndTribe)
                .WithMany(p => p.Offices)
                .HasForeignKey(p => p.CountyandTribeId);

            HasOptional(p => p.ContractArea)
                .WithMany(p => p.Offices)
                .HasForeignKey(p => p.ContractAreaId);

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

            Property(p => p.MFLocationNumber)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.CountyandTribeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContractAreaId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ActiviatedDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.InactivatedDate)
                .HasColumnType("date")
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

            #endregion
        }
    }
}
