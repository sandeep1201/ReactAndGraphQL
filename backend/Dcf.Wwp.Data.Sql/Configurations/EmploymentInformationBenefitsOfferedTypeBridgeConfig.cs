using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmploymentInformationBenefitsOfferedTypeBridgeConfig : BaseCommonModelConfig<EmploymentInformationBenefitsOfferedTypeBridge>
    {
        public EmploymentInformationBenefitsOfferedTypeBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.EmploymentInformation)
                .WithMany(p => p.EmploymentInformationBenefitsOfferedTypeBridges)
                .HasForeignKey(p => p.EmploymentInformationId);

            HasOptional(p => p.BenefitsOfferedType)
                .WithMany()
                .HasForeignKey(p => p.BenefitsOfferedTypeId);

            #endregion

            #region Properties

            ToTable("EmploymentInformationBenefitsOfferedTypeBridge");

            Property(p => p.EmploymentInformationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BenefitsOfferedTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SortOrder)
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

            #endregion
        }
    }
}
