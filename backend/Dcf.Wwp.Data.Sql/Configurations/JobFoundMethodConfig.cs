using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class JobFoundMethodConfig : BaseCommonModelConfig<JobFoundMethod>
    {
        public JobFoundMethodConfig()
        {
            #region Relationships

            HasMany(p => p.OtherJobInformations)
                .WithOptional(p => p.JobFoundMethod)
                .HasForeignKey(p => p.JobFoundMethodId);

            #endregion

            #region Properties

            ToTable("JobFoundMethod");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
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
