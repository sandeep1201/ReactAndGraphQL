using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class JobTypeConfig : BaseCommonModelConfig<JobType>
    {
        public JobTypeConfig()
        {
            #region Relationships

            HasMany(p => p.EmploymentInformations)
                .WithOptional(p => p.JobType)
                .HasForeignKey(p => p.JobTypeId);

            HasMany(p => p.JobTypeLeavingReasonBridges)
                .WithRequired(p => p.JobType)
                .HasForeignKey(p => p.JobTypeId);

            #endregion

            #region Properties

            ToTable("JobType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.IsRequired)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsUsedForEmploymentOfRecord)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
