using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class JobDutiesDetailsConfig : BaseCommonModelConfig<JobDutiesDetail>
    {
        public JobDutiesDetailsConfig()
        {
            #region Relationships

            HasMany(p => p.EmploymentInformationJobDutiesDetailsBridges)
                .WithOptional(p => p.JobDutiesDetail)
                .HasForeignKey(p => p.JobDutiesId);

            #endregion

            #region Properties

            ToTable("JobDutiesDetails");

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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
