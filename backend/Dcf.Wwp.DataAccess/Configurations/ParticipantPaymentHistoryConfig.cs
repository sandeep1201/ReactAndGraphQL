using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ParticipantPaymentHistoryConfig : BaseConfig<ParticipantPaymentHistory>
    {
        public ParticipantPaymentHistoryConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            ToTable("ParticipantPaymentHistory");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CaseNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.EffectiveMonth)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipationBeginDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.BaseW2Payment)
                .HasColumnType("decimal")
                .IsRequired();

            Property(p => p.DrugFelonPenalty)
                .HasColumnType("decimal")
                .IsRequired();

            Property(p => p.Recoupment)
                .HasColumnType("decimal")
                .IsRequired();

            Property(p => p.LearnFarePenalty)
                .HasColumnType("decimal")
                .IsRequired();

            Property(p => p.AdjustedBasePayment)
                .HasColumnType("decimal")
                .IsOptional();

            Property(p => p.NonParticipationReduction)
                .HasColumnType("decimal")
                .IsRequired();

            Property(p => p.OverPayment)
                .HasColumnType("decimal")
                .IsOptional();

            Property(p => p.FinalPayment)
                .HasColumnType("decimal")
                .IsOptional();

            Property(p => p.VendorPayment)
                .HasColumnType("decimal")
                .IsRequired();

            Property(p => p.ParticipantPayment)
                .HasColumnType("decimal")
                .IsRequired();

            Property(p => p.ParticipationEndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.IsOriginal)
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
                .IsRequired();

            #endregion
        }
    }
}
