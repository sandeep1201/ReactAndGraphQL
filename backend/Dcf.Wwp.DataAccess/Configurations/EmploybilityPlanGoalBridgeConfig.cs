using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EmployabilityPlanGoalBridgeConfig : BaseConfig<EmployabilityPlanGoalBridge>
    {
        public EmployabilityPlanGoalBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EmployabilityPlan)
                .WithMany(p => p.EmployabilityPlanGoalBridges)
                .HasForeignKey(p => p.EmployabilityPlanId)
                .WillCascadeOnDelete(true);

            HasRequired(p => p.Goal)
                .WithMany(p => p.EmployabilityPlanGoalBridges)
                .HasForeignKey(p => p.GoalId)
                .WillCascadeOnDelete(true);

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
