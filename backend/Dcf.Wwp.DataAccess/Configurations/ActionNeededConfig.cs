using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ActionNeededConfig : BaseConfig<ActionNeeded>
    {
        public ActionNeededConfig()
        {
            #region Relationships

            HasRequired(p => p.ActionNeededPage)
                .WithMany()
                .HasForeignKey(p => p.ActionNeededPageId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ActionNeededTasks)
                .WithRequired(p => p.ActionNeeded)
                .HasForeignKey(p => p.ActionNeededId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("ActionNeeded");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ActionNeededPageId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsNoActionNeeded)
                .HasColumnType("bit")
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
