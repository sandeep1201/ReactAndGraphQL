using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class T0459_IN_W2_LIMITS_COMPAREConfig : EntityTypeConfiguration<T0459_IN_W2_LIMITS_COMPARE>
    {
        public T0459_IN_W2_LIMITS_COMPAREConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("T0459_IN_W2_LIMITS_COMPARE");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PIN_NUM)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.BENEFIT_MM)
                .HasColumnType("decimal")
                .HasPrecision(6, 0)
                .IsRequired();

            Property(p => p.HISTORY_SEQ_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.CLOCK_TYPE_CD)
                .HasColumnType("char")
                .HasMaxLength(4)
                .IsRequired();

            Property(p => p.CRE_TRAN_CD)
                .HasColumnType("char")
                .HasMaxLength(8)
                .IsRequired();

            Property(p => p.FED_CLOCK_IND)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired();

            Property(p => p.FED_CMP_MTH_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.FED_MAX_MTH_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.HISTORY_CD)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.OT_CMP_MTH_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.OVERRIDE_REASON_CD)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsRequired();

            Property(p => p.TOT_CMP_MTH_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.TOT_MAX_MTH_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.UPDATED_DT)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.USER_ID)
                .HasColumnType("char")
                .HasMaxLength(6)
                .IsRequired();

            Property(p => p.WW_CMP_MTH_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.WW_MAX_MTH_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.COMMENT_TXT)
                .HasColumnType("varchar")
                .HasMaxLength(75)
                .IsRequired();

            #endregion
        }
    }
}
