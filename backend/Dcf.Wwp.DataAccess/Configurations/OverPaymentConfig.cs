using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class OverPaymentConfig : BaseConfig<OverPayment>
    {
        public OverPaymentConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.OverPayments)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.Office)
                .WithMany()
                .HasForeignKey(p => p.OfficeId);

            #endregion

            #region Properties

            ToTable("OverPayment");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipationPeriodBeginDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.ParticipationPeriodEndDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.CaseNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.Amount)
                .HasColumnType("decimal")
                .HasPrecision(5, 2)
                .IsRequired();

            Property(p => p.OfficeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Reason)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

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
                .IsRequired();

            #endregion
        }
    }
}
