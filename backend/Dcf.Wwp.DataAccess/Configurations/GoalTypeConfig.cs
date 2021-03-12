using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class GoalTypeConfig : BaseConfig<GoalType>
    {
        public GoalTypeConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany(p => p.GoalTypes)
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("GoalType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EnrolledProgramId)
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
