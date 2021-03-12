using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class TimeLimitConfig : BaseConfig<TimeLimit>
    {
        public TimeLimitConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.TimeLimits)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("TimeLimit");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EffectiveMonth)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.TimeLimitTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TwentyFourMonthLimit)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.StateTimelimit)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.FederalTimeLimit)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.StateId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ChangeReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ChangeReasonDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.PIN_NUM)
                .HasColumnType("decimal")
                .IsOptional();

            #endregion
        }
    }
}
