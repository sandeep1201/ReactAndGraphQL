using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmploybilityPlanActivityBridgeConfig : BaseCommonModelConfig<EmployabilityPlanActivityBridge>
    {
        public EmploybilityPlanActivityBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EmployabilityPlan)
                .WithMany(p => p.EmployabilityPlanActivityBridges)
                .HasForeignKey(p => p.EmployabilityPlanId);

            HasRequired(p => p.Activity)
                .WithMany(p => p.EmployabilityPlanActivityBridges)
                .HasForeignKey(p => p.ActivityId);

            #endregion

            #region Properties

            ToTable("EmployabilityPlanActivityBridge");

            Property(p => p.EmployabilityPlanId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActivityId)
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
