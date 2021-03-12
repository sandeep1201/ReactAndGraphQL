using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class GoalTypeConfig : BaseCommonModelConfig<GoalType>
    {
        public GoalTypeConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany(p => p.GoalTypes)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasMany(p => p.Goals)
                .WithOptional(p => p.GoalType)
                .HasForeignKey(p => p.GoalTypeId);

            #endregion

            #region Properties

            ToTable("GoalType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
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
