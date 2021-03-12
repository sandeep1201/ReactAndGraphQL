using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class PlanSectionResourceConfig : BaseConfig<PlanSectionResource>
    {
        public PlanSectionResourceConfig()
        {
            #region Relationships

            HasRequired(p => p.PlanSection)
                .WithMany(p => p.PlanSectionResources)
                .HasForeignKey(p => p.PlanSectionId);

            #endregion

            #region Properties

            ToTable("PlanSectionResource");

            Property(p => p.PlanSectionId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Resource)
                .HasColumnType("varchar")
                .HasMaxLength(300)
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
