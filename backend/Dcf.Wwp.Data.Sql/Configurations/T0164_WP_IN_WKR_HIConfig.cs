using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class T0164_WP_IN_WKR_HIConfig : EntityTypeConfiguration<T0164_WP_IN_WKR_HI>
    {
        public T0164_WP_IN_WKR_HIConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("T0164_WP_IN_WKR_HI");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PIN_NUM)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.EMPLOYER_SEQ_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.HISTORY_SEQ_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.AVG_WK_HRS)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.CRE_TMS)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.DEL_IND)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired();

            Property(p => p.DURATION_EMP_IND)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired();

            Property(p => p.EE_IND)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired();

            Property(p => p.EMP_CITY_ADR)
                .HasColumnType("char")
                .HasMaxLength(15)
                .IsRequired();

            Property(p => p.EMP_LINE_1_ADR)
                .HasColumnType("char")
                .HasMaxLength(30)
                .IsRequired();

            Property(p => p.EMP_LINE_2_ADR)
                .HasColumnType("char")
                .HasMaxLength(30)
                .IsRequired();

            Property(p => p.EMP_STATE_ADR)
                .HasColumnType("char")
                .HasMaxLength(2)
                .IsRequired();

            Property(p => p.EMP_TYPE_CD)
                .HasColumnType("char")
                .HasMaxLength(2)
                .IsRequired();

            Property(p => p.EMP_ZIP_ADR)
                .HasColumnType("char")
                .HasMaxLength(9)
                .IsRequired();

            Property(p => p.EMPLOYER_NAM)
                .HasColumnType("char")
                .HasMaxLength(30)
                .IsRequired();

            Property(p => p.EMPLOYMENT_BEG_DT)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.EMPLOYMENT_END_DT)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.HISTORY_CD)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.HOURLY_WAGE_AMT)
                .HasColumnType("decimal")
                .HasPrecision(5, 2)
                .IsRequired();

            Property(p => p.JOB_CD)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsRequired();

            Property(p => p.JOB_DUTIES_1_TXT)
                .HasColumnType("char")
                .HasMaxLength(35)
                .IsRequired();

            Property(p => p.JOB_DUTIES_2_TXT)
                .HasColumnType("char")
                .HasMaxLength(35)
                .IsRequired();

            Property(p => p.JOB_DUTIES_3_TXT)
                .HasColumnType("char")
                .HasMaxLength(35)
                .IsRequired();

            Property(p => p.MEDICAL_BEN_IND)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired();

            Property(p => p.OFFICE_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.OT_BEN_CD)
                .HasColumnType("char")
                .HasMaxLength(2)
                .IsRequired();

            Property(p => p.PAY_CD)
                .HasColumnType("char")
                .HasMaxLength(2)
                .IsRequired();

            Property(p => p.PROVIDER_ID)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.STAFF_ID)
                .HasColumnType("char")
                .HasMaxLength(6)
                .IsRequired();

            Property(p => p.UPDT_TMS)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.USER_ID)
                .HasColumnType("char")
                .HasMaxLength(6)
                .IsRequired();

            Property(p => p.WORK_LEFT_CD)
                .HasColumnType("char")
                .HasMaxLength(2)
                .IsRequired();

            Property(p => p.HRS_OR_WAGE_CHG_DT)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.JOB_TYP)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired();

            Property(p => p.RES_MILW_ITIV_IND)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired();

            #endregion
        }
    }
}
