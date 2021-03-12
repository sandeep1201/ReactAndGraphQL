using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class AuxiliaryPaymentConfig : EntityTypeConfiguration<AuxiliaryPayment>
    {
        public AuxiliaryPaymentConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("AuxiliaryPayment");

            Property(p => p.PinNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.EffectiveMonth)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.TimeLimitTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StateTimelimit)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.FederalTimeLimit)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.TwentyFourMonthLimit)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CreatedDateFromCARES)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.PIN_NUM)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.BENEFIT_MM)
                .HasColumnType("decimal")
                .HasPrecision(6, 0)
                .IsOptional();

            Property(p => p.HISTORY_SEQ_NUM)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.CLOCK_TYPE_CD)
                .HasColumnType("char")
                .HasMaxLength(4)
                .IsOptional();

            Property(p => p.CRE_TRAN_CD)
                .HasColumnType("char")
                .HasMaxLength(8)
                .IsOptional();

            Property(p => p.FED_CLOCK_IND)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.FED_CMP_MTH_NUM)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.FED_MAX_MTH_NUM)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.HISTORY_CD)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.OT_CMP_MTH_NUM)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.OVERRIDE_REASON_CD)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.TOT_CMP_MTH_NUM)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.TOT_MAX_MTH_NUM)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.UPDATED_DT)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.USER_ID)
                .HasColumnType("char")
                .HasMaxLength(6)
                .IsOptional();

            Property(p => p.WW_CMP_MTH_NUM)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.WW_MAX_MTH_NUM)
                .HasColumnType("smallint")
                .IsOptional();

            Property(p => p.COMMENT_TXT)
                .HasColumnType("varchar")
                .HasMaxLength(75)
                .IsOptional();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Ignore(p => p.IsDeleted);
            Ignore(p => p.CreatedDate);

            #endregion
        }
    }
}
