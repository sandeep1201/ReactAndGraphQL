using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ActionPriorityConfig : BaseConfig<ActionPriority>
    {
        public ActionPriorityConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            ToTable("ActionPriority");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
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
