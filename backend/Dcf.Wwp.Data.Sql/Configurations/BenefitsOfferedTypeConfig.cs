using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class BenefitsOfferedTypeConfig : BaseCommonModelConfig<BenefitsOfferedType>
    {
        public BenefitsOfferedTypeConfig()
        {
            #region Relationships

            HasMany(p => p.EmploymentInformationBenefitsOfferedTypeBridges)
                .WithOptional(p => p.BenefitsOfferedType)
                .HasForeignKey(p => p.BenefitsOfferedTypeId);

            #endregion

            #region Properties

            ToTable("BenefitsOfferedType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.DisablesOthersFlag)
                .HasColumnType("bit")
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
