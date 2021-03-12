using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class AfterPullDownWeeklyBatchDetailsConfig : BaseConfig<AfterPullDownWeeklyBatchDetails>
    {
        public AfterPullDownWeeklyBatchDetailsConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            #endregion

            #region Properties

            ToTable("AfterPullDownWeeklyBatchDetails");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CaseNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.ParticipationBeginDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.ParticipationEndDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.WeeklyBatchDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.PreviousNPHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsRequired();

            Property(p => p.PreviousGCHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsRequired();

            Property(p => p.CurrentNPHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsRequired();

            Property(p => p.CurrentGCHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsRequired();

            Property(p => p.PreviousUnAppliedHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsOptional();

            Property(p => p.CurrentUnAppliedHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
                .IsOptional();

            Property(p => p.Calculation)
                .HasColumnType("decimal")
                .HasPrecision(5, 2)
                .IsRequired();

            Property(p => p.OverPaymentOrAux)
                .HasColumnType("varchar")
                .HasMaxLength(100)
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
