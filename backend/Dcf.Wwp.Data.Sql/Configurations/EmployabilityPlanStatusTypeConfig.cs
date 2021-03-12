using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmployabilityPlanStatusTypeConfig : BaseCommonModelConfig<EmployabilityPlanStatusType>
    {
        public EmployabilityPlanStatusTypeConfig()
        {
            #region Relationships

            HasMany(p => p.EmployabilityPlans)
                .WithOptional(p => p.EmployabilityPlanStatusType)
                .HasForeignKey(p => p.EmployabilityPlanStatusTypeId);

            #endregion

            #region Properties

            ToTable("EmployabilityPlanStatusType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
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
                .IsRequired();

            #endregion
        }
    }
}
