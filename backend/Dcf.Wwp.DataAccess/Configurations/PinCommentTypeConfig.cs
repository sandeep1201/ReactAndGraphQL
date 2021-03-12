using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class PinCommentTypeConfig : BaseConfig<PinCommentType>
    {
        public PinCommentTypeConfig()
        {
            #region Relationships

            HasMany(p => p.PCCTBridges)
                .WithRequired(p => p.PinCommentType)
                .HasForeignKey(p => p.CommentTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("PinCommentType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
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

            Property(p => p.SystemUseOnly)
                .HasColumnType("bit")
                .IsRequired();

            #endregion
        }
    }
}
