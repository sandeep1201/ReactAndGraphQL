using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class PlanStatusTypeConfig : BaseConfig<PlanStatusType>
    {
        public PlanStatusTypeConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            ToTable("PlanStatusType");

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsRequired();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsSystemUseOnly)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.EffectiveDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.EndDate)
                .HasColumnType("date")
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
