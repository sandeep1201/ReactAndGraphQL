using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActivityLocationConfig : BaseCommonModelConfig<ActivityLocation>
    {
        public ActivityLocationConfig()
        {
            #region Relationships

            HasMany(p => p.Activities)
                .WithOptional(p => p.ActivityLocation)
                .HasForeignKey(p => p.ActivityLocationId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("ActivityLocation");

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
