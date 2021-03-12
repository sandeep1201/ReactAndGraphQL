using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TimeLimitClosureLogConfig : BaseConfig<TimelimitClosureLog>
    {
        public TimeLimitClosureLogConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            #endregion

            #region Properties

            ToTable("TimeLimitClosureLog");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.TargetDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.MaxedTimelimitTypes)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
