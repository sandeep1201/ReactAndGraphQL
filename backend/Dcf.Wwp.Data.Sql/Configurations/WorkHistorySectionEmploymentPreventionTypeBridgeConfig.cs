using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WorkHistorySectionEmploymentPreventionTypeBridgeConfig : BaseCommonModelConfig<WorkHistorySectionEmploymentPreventionTypeBridge>
    {
        public WorkHistorySectionEmploymentPreventionTypeBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.WorkHistorySection)
                .WithMany(p => p.WorkHistorySectionEmploymentPreventionTypeBridges)
                .HasForeignKey(p => p.WorkHistorySectionId);

            HasRequired(p => p.EmploymentPreventionType)
                .WithMany()
                .HasForeignKey(p => p.EmploymentPreventionTypeId);

            #endregion

            #region Properties

            ToTable("WorkHistorySectionEmploymentPreventionTypeBridge");

            Property(p => p.WorkHistorySectionId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmploymentPreventionTypeId)
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
