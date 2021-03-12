using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WorkProgramStatusConfig : BaseCommonModelConfig<WorkProgramStatus>
    {
        public WorkProgramStatusConfig()
        {
            #region Relationships

            HasMany(p => p.InvolvedWorkPrograms)
                .WithOptional(p => p.WorkProgramStatus)
                .HasForeignKey(p => p.WorkProgramStatusId);

            #endregion

            #region Properties

            ToTable("WorkProgramStatus");

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
                .IsOptional();

            #endregion
        }
    }
}
