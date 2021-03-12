using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAHouseHoldIncomeConfig : BaseConfig<EAHouseHoldIncome>
    {
        public EAHouseHoldIncomeConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaHouseHoldIncomes)
                .HasForeignKey(p => p.RequestId);

            HasRequired(p => p.EaVerificationType)
                .WithMany()
                .HasForeignKey(p => p.VerificationTypeId);

            HasRequired(p => p.Participant)
                .WithMany(p => p.EaHouseHoldIncomes)
                .HasForeignKey(p => p.GroupMember);

            #endregion

            #region Properties

            ToTable("EAHouseHoldIncome");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IncomeType)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.MonthlyIncome)
                .HasColumnType("decimal")
                .HasPrecision(8, 2)
                .IsOptional();

            Property(p => p.VerificationTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.GroupMember)
                .HasColumnType("int")
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
