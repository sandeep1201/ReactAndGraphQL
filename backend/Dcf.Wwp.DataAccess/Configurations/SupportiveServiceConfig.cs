using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class SupportiveServiceConfig : BaseConfig<SupportiveService>
    {
        public SupportiveServiceConfig()
        {
            #region Relationships

            HasRequired(p => p.EmployabilityPlan)
                .WithMany(p => p.SupportiveServices)
                .HasForeignKey(p => p.EmployabilityPlanId)
                .WillCascadeOnDelete(false); 

            HasRequired(p => p.SupportiveServiceType)
                .WithMany()
                .HasForeignKey(p => p.SupportiveServiceTypeId)
                .WillCascadeOnDelete(false);

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
