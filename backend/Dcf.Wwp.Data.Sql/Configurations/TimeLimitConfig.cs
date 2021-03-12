using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TimeLimitConfig : BaseCommonModelConfig<TimeLimit>
    {
        public TimeLimitConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.TimeLimits)
                .HasForeignKey(p => p.ParticipantID);

            HasOptional(p => p.TimeLimitType)
                .WithMany(p => p.TimeLimits)
                .HasForeignKey(p => p.TimeLimitTypeId);

            HasOptional(p => p.TimeLimitState)
                .WithMany()
                .HasForeignKey(p => p.StateId);

            HasOptional(p => p.ChangeReason)
                .WithMany(p => p.TimeLimits)
                .HasForeignKey(p => p.ChangeReasonId);

            #endregion

            #region Properties

            ToTable("TimeLimit");

            Property(p => p.ParticipantID)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PIN_NUM)
                .HasColumnName("PIN_NUM")
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
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

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Ignore(p => p.PinNumber);

            #endregion
        }
    }
}
