using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActivityContactBridgeConfig : BaseCommonModelConfig<ActivityContactBridge>
    {
        public ActivityContactBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.Activity)
                .WithMany(p => p.ActivityContactBridges)
                .HasForeignKey(p => p.ActivityId);

            HasOptional(p => p.Contact)
                .WithMany(p => p.ActivityContactBridges)
                .HasForeignKey(p => p.ContactId);

            #endregion

            #region Properties

            ToTable("ActivityContactBridge");

            Property(p => p.ActivityId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContactId)
                .HasColumnType("int")
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
                .IsOptional();

            #endregion
        }
    }
}
