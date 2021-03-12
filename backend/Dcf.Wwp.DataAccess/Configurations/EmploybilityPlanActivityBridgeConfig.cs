using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EmployabilityPlanActivityBridgeConfig : BaseConfig<EmployabilityPlanActivityBridge>
    {
        public EmployabilityPlanActivityBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EmployabilityPlan)
                .WithMany(p => p.EmploybilityPlanActivityBridges)
                .HasForeignKey(p => p.EmployabilityPlanId)
                .WillCascadeOnDelete(true);

            HasRequired(p => p.Activity)
                .WithMany(p => p.EmploybilityPlanActivityBridges)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

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
