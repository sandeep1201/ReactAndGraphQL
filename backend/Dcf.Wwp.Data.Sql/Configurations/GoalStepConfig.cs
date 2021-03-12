using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class GoalStepConfig : BaseCommonModelConfig<GoalStep>
    {
        public GoalStepConfig()
        {
            #region Relationships

            HasRequired(p => p.Goal)
                .WithMany(p => p.GoalSteps)
                .HasForeignKey(p => p.GoalId);

            #endregion

            #region Properties

            ToTable("GoalStep");

            Property(p => p.GoalId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

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
