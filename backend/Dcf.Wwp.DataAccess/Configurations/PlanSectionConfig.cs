using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class PlanSectionConfig : BaseConfig<PlanSection>
    {
        public PlanSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Plan)
                .WithMany(p => p.PlanSections)
                .HasForeignKey(p => p.PlanId);

            HasRequired(p => p.PlanSectionType)
                .WithMany()
                .HasForeignKey(p => p.PlanSectionTypeId);

            HasMany(p => p.PlanSectionResources)
                .WithRequired(p => p.PlanSection)
                .HasForeignKey(p => p.PlanSectionId);

            #endregion

            #region Properties

            ToTable("PlanSection");

            Property(p => p.PlanId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PlanSectionTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsNotNeeded)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ShortTermPlanOfAction)
                .HasColumnType("varchar")
                .HasMaxLength(900)
                .IsOptional();

            Property(p => p.LongTermPlanOfAction)
                .HasColumnType("varchar")
                .HasMaxLength(900)
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
