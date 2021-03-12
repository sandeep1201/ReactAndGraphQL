using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EACommentTypeBridgeConfig : BaseConfig<EACommentTypeBridge>
    {
        public EACommentTypeBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EaComment)
                .WithMany(p => p.EaCommentTypeBridges)
                .HasForeignKey(p => p.CommentId);

            HasRequired(p => p.EaCommentType)
                .WithMany(p => p.EaCommentTypeBridges)
                .HasForeignKey(p => p.CommentTypeId);

            #endregion

            #region Properties

            ToTable("EACommentTypeBridge");

            Property(p => p.CommentId)
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
                .IsRequired();

            #endregion
        }
    }
}
