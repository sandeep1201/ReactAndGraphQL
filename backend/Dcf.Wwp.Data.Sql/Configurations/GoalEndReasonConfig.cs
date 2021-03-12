using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class GoalEndReasonConfig : BaseCommonModelConfig<GoalEndReason>
    {
        public GoalEndReasonConfig()
        {
            #region Relationships

            HasMany(p => p.Goals)
                .WithOptional(p => p.GoalEndReason)
                .HasForeignKey(p => p.GoalEndReasonId);

            #endregion

            #region Properties

            ToTable("GoalEndReason");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.SortOrder)
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
