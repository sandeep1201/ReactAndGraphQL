using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ActivityTypeConfig : BaseConfig<ActivityType>
    {
        public ActivityTypeConfig()
        {
            #region Relationships

            HasMany(p => p.EnrolledProgramEPActivityTypeBridges)
                .WithOptional(p => p.ActivityType)
                .HasForeignKey(p => p.ActivityTypeId)
                .WillCascadeOnDelete(false);

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
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
