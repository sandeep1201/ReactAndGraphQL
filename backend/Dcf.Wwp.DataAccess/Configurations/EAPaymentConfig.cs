using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAPaymentConfig : BaseConfig<EAPayment>
    {
        public EAPaymentConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaPayments)
                .HasForeignKey(p => p.RequestId);

            HasOptional(p => p.EaAlternateMailingAddress)
                .WithMany(p => p.EaPayments)
                .HasForeignKey(p => p.MailingAddressId);

            #endregion

            #region Properties

            ToTable("EAPayment");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.VoucherOrCheckNumber)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.VoucherOrCheckDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.VoucherOrCheckAmount)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.PayeeName)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.MailingAddressId)
                .HasColumnType("int")
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
                .IsRequired();

            #endregion
        }
    }
}
