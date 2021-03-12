using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class GoalConfig : BaseConfig<Goal>
    {
        public GoalConfig()
        {
            #region Relationships

            HasOptional(p => p.GoalType)
                .WithMany()
                .HasForeignKey(p => p.GoalTypeId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.GoalEndReason)
                .WithMany()
                .HasForeignKey(p => p.GoalEndReasonId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.GoalSteps)
                .WithRequired(p => p.Goal)
                .HasForeignKey(p => p.GoalId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.EmployabilityPlanGoalBridges)
                .WithRequired(p => p.Goal)
                .HasForeignKey(p => p.GoalId)
                .WillCascadeOnDelete(true);

            #endregion

            #region Properties

            ToTable("Goal");

            Property(p => p.GoalTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BeginDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.IsGoalEnded)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.GoalEndReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EndReasonDetails)
                .HasColumnType("varchar")
                .HasMaxLength(500)
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
