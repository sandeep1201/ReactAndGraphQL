using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EPEIBridgeConfig : BaseCommonModelConfig<EPEIBridge>
    {
        public EPEIBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.EmployabilityPlan)
                .WithMany(p => p.EPEIBridges)
                .HasForeignKey(p => p.EmployabilityPlanId);

            HasOptional(p => p.EmploymentInformation)
                .WithMany(p => p.EPEIBridges)
                .HasForeignKey(p => p.EmploymentInformationId);

            #endregion

            #region Properties

            ToTable("EPEIBridge");

            Property(p => p.EmployabilityPlanId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EmploymentInformationId)
                .HasColumnType("int")
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

            #endregion
        }
    }
}
