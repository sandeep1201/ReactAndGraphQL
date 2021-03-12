using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class PCCTBridgeConfig : BaseConfig<PCCTBridge>
    {
        public PCCTBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.PinComment)
                .WithMany(p => p.PCCTBridges)
                .HasForeignKey(p => p.PinCommentId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.PinCommentType)
                .WithMany(p => p.PCCTBridges)
                .HasForeignKey(p => p.CommentTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("PCCTBridge");

            Property(p => p.PinCommentId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CommentTypeId)
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
                .IsOptional();

            #endregion
        }
    }
}
