using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmployabilityPlanGoalBridgeConfig : BaseCommonModelConfig<EmployabilityPlanGoalBridge>
    {
        public EmployabilityPlanGoalBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EmployabilityPlan)
                .WithMany(p => p.EmployabilityPlanGoalBridges)
                .HasForeignKey(p => p.EmployabilityPlanId);

            HasRequired(p => p.Goal)
                .WithMany(p => p.EmployabilityPlanGoalBridges)
                .HasForeignKey(p => p.GoalId);

            #endregion

            #region Properties

            ToTable("EmployabilityPlanGoalBridge");

            Property(p => p.EmployabilityPlanId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.GoalId)
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
