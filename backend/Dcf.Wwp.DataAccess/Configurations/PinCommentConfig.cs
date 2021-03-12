using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class PinCommentConfig : BaseConfig<PinComment>
    {
        public PinCommentConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.PinComments)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("PinComment");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsEdited)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.CommentText)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
