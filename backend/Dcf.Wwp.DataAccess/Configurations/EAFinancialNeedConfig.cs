using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAFinancialNeedConfig : BaseConfig<EAFinancialNeed>
    {
        public EAFinancialNeedConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaFinancialNeeds)
                .HasForeignKey(p => p.RequestId);

            HasRequired(p => p.EaFinancialNeedType)
                .WithMany()
                .HasForeignKey(p => p.FinancialNeedTypeId);

            #endregion

            #region Properties

            ToTable("EAFinancialNeed");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.FinancialNeedTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Amount)
                .HasColumnType("decimal")
                .HasPrecision(8, 2)
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
