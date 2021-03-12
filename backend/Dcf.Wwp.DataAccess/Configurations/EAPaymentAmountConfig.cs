using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAPaymentAmountConfig : BaseConfig<EAPaymentAmount>
    {
        public EAPaymentAmountConfig()
        {
            #region Relationships

            HasRequired(p => p.EaEmergencyType)
                .WithMany()
                .HasForeignKey(p => p.EmergencyTypeId);

            #endregion

            #region Properties

            ToTable("EAPaymentAmount");

            Property(p => p.EmergencyTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.MinGroupSize)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.MaxGroupSize)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.AmountPerMember)
                .HasColumnType("decimal")
                .HasPrecision(5, 2)
                .IsRequired();

            Property(p => p.MaxPaymentAmount)
                .HasColumnType("decimal")
                .HasPrecision(5, 2)
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
