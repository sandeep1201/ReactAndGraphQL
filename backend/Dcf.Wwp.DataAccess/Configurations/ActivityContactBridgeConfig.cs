using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ActivityContactBridgeConfig : BaseConfig<ActivityContactBridge>
    {
        public ActivityContactBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.Activity)
                .WithMany(p => p.ActivityContactBridges)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(true);

            HasOptional(p => p.Contact)
                .WithMany(p => p.ActivityContactBridges)
                .HasForeignKey(p => p.ContactId)
                .WillCascadeOnDelete(false);

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
