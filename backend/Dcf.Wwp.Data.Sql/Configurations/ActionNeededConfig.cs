using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ActionNeededConfig : BaseCommonModelConfig<ActionNeeded>
    {
        public ActionNeededConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.ActionNeededPage)
                .WithMany(p => p.ActionNeededs)
                .HasForeignKey(p => p.ActionNeededPageId);

            HasMany(p => p.ActionNeededTasks)
                .WithRequired(p => p.ActionNeeded)
                .HasForeignKey(p => p.ActionNeededId);

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

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
