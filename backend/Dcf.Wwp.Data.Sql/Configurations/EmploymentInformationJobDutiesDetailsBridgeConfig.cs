using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmploymentInformationJobDutiesDetailsBridgeConfig : BaseCommonModelConfig<EmploymentInformationJobDutiesDetailsBridge>
    {
        public EmploymentInformationJobDutiesDetailsBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.EmploymentInformation)
                .WithMany(p => p.EmploymentInformationJobDutiesDetailsBridges)
                .HasForeignKey(p => p.EmploymentInformationId);

            HasOptional(p => p.JobDutiesDetail)
                .WithMany()
                .HasForeignKey(p => p.JobDutiesId);

            #endregion

            #region Properties

            ToTable("EmploymentInformationJobDutiesDetailsBridge");

            Property(p => p.EmploymentInformationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.JobDutiesId)
                .HasColumnType("int")
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
