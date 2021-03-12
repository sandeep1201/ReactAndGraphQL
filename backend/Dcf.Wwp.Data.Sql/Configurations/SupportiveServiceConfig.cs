using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class SupportiveServiceConfig : BaseCommonModelConfig<SupportiveService>
    {
        public SupportiveServiceConfig()
        {
            #region Relationships

            HasRequired(p => p.EmployabilityPlan)
                .WithMany(p => p.SupportiveServices)
                .HasForeignKey(p => p.EmployabilityPlanId);

            HasRequired(p => p.SupportiveServiceType)
                .WithMany()
                .HasForeignKey(p => p.SupportiveServiceTypeId);

            #endregion

            #region Properties

            ToTable("SupportiveService");

            Property(p => p.EmployabilityPlanId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.SupportiveServiceTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(500)
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

            #endregion
        }
    }
}
