using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class PlanConfig : BaseConfig<Plan>
    {
        public PlanConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.Plans)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.PlanType)
                .WithMany()
                .HasForeignKey(p => p.PlanTypeid);

            HasRequired(p => p.PlanStatusType)
                .WithMany()
                .HasForeignKey(p => p.PlanStatusTypeid);

            HasRequired(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId);

            HasMany(p => p.PlanSections)
                .WithRequired(p => p.Plan)
                .HasForeignKey(p => p.PlanId);

            #endregion

            #region Properties

            ToTable("Plan");

            Property(p => p.PlanNumber)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PlanTypeid)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PlanStatusTypeid)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
