using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ActivityCompletionReasonConfig : BaseConfig<ActivityCompletionReason>
    {
        public ActivityCompletionReasonConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            ToTable("ActivityCompletionReason");

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsRequired();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.IsSystemUseOnly)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EffectiveDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
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
