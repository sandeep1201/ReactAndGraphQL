using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class WorkerTaskCategoryConfig : BaseConfig<WorkerTaskCategory>
    {
        public WorkerTaskCategoryConfig()
        {
            #region Relationships

            //HasMany(p => p.WorkerTaskList)
            //    .WithRequired(p => p.WorkerTaskCategory)
            //    .HasForeignKey(p => p.CategoryId)
            //    .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("WorkerTaskCategory");

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsRequired();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsSystemUseOnly)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.EffectiveDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
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

            Property(p => p.Description)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.IsWWLF)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsCF)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsTJTMJ)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsFCDP)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsEA)
                .HasColumnType("bit")
                .IsRequired();

            #endregion
        }
    }
}
