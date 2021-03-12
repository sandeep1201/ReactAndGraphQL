using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActivityTypeConfig : BaseCommonModelConfig<ActivityType>
    {
        public ActivityTypeConfig()
        {
            #region Relationships

            HasMany(p => p.Activities)
                .WithRequired(p => p.ActivityType)
                .HasForeignKey(p => p.ActivityTypeId);

            HasMany(p => p.EnrolledProgramEPActivityTypeBridges)
                .WithRequired(p => p.ActivityType)
                .HasForeignKey(p => p.ActivityTypeId);

            #endregion

            #region Properties

            ToTable("ActivityType");

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsRequired();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(255)
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

            Property(p => p.EffectiveDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.EndDate)
                .HasColumnType("date")
                .IsRequired();

            #endregion
        }
    }
}
