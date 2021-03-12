using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EmployabilityPlanEmploymentInfoBridgeConfig : BaseConfig<EmployabilityPlanEmploymentInfoBridge>
    {
        public EmployabilityPlanEmploymentInfoBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EmployabilityPlan)
                .WithMany(p => p.EmploybilityPlanEmploymentInfoBridges)
                .HasForeignKey(p => p.EmployabilityPlanId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.EmploymentInformation)
                .WithMany(p => p.EmploybilityPlanEmploymentInfoBridges)
                .HasForeignKey(p => p.EmploymentInformationId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("EPEIBridge");

            Property(p => p.Id)
               .HasColumnType("int")
               .IsOptional();

            Property(p => p.EmployabilityPlanId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmploymentInformationId)
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
