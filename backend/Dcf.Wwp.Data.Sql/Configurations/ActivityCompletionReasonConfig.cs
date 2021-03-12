using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActivityCompletionReasonConfig : BaseConfig<ActivityCompletionReason>
    {
        public ActivityCompletionReasonConfig()
        {
            #region Relationships

            // none

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

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsSystemUseOnly)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EffectiveDate)
                .HasColumnType("date")
                .IsOptional();

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
