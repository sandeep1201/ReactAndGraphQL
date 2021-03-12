using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class GoalStepConfig : BaseConfig<GoalStep>
    {
        public GoalStepConfig()
        {
            #region Relationships

            HasRequired(p => p.Goal)
                .WithMany(p => p.GoalSteps)
                .HasForeignKey(p => p.GoalId)
                .WillCascadeOnDelete(true);

            #endregion

            #region Properties

            ToTable("GoalStep");

            Property(p => p.GoalId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsRequired();

            Property(p => p.IsGoalStepCompleted)
                .HasColumnType("bit")
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
