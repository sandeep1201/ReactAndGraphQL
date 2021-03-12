using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class T0459_IN_W2_LIMITSConfig : EntityTypeConfiguration<T0459_IN_W2_LIMITS>
    {
        public T0459_IN_W2_LIMITSConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("T0459_IN_W2_LIMITS");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PIN_NUM)
                .HasColumnName("PIN_NUM")
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.BENEFIT_MM)
                .HasColumnName("BENEFIT_MM")
                .HasColumnType("decimal")
                .HasPrecision(6, 0)
                .IsRequired();

            Property(p => p.HISTORY_SEQ_NUM)
                .HasColumnName("HISTORY_SEQ_NUM")
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.CLOCK_TYPE_CD)
                .HasColumnName("CLOCK_TYPE_CD")
                .HasColumnType("varchar")
                .HasMaxLength(4)
                .IsRequired();

            Property(p => p.CRE_TRAN_CD)
                .HasColumnName("CRE_TRAN_CD")
                .HasColumnType("char")
                .HasMaxLength(8)
                .IsRequired();

            Property(p => p.FED_CLOCK_IND)
                .HasColumnName("FED_CLOCK_IND")
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired();

            Property(p => p.FED_CMP_MTH_NUM)
                .HasColumnName("FED_CMP_MTH_NUM")
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.FED_MAX_MTH_NUM)
                .HasColumnName("FED_MAX_MTH_NUM")
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.HISTORY_CD)
                .HasColumnName("HISTORY_CD")
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.OT_CMP_MTH_NUM)
                .HasColumnName("OT_CMP_MTH_NUM")
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.OVERRIDE_REASON_CD)
                .HasColumnName("OVERRIDE_REASON_CD")
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsRequired();

            Property(p => p.TOT_CMP_MTH_NUM)
                .HasColumnName("TOT_CMP_MTH_NUM")
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.TOT_MAX_MTH_NUM)
                .HasColumnName("TOT_MAX_MTH_NUM")
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.COMMENT_TXT)
                .HasColumnName("COMMENT_TXT")
                .HasColumnType("varchar")
                .HasMaxLength(75)
                .IsRequired();

            Property(p => p.UPDATED_DT)
                .HasColumnName("UPDATED_DT")
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.USER_ID)
                .HasColumnName("USER_ID")
                .HasColumnType("char")
                .HasMaxLength(6)
                .IsRequired();

            Property(p => p.WW_CMP_MTH_NUM)
                .HasColumnName("WW_CMP_MTH_NUM")
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.WW_MAX_MTH_NUM)
                .HasColumnName("WW_MAX_MTH_NUM")
                .HasColumnType("smallint")
                .IsRequired();

            #endregion

            #region Sproc Mapping

            MapToStoredProcedures(p => p.Insert(sp => sp.HasName("DB2_T0459_Insert")
                                                        .Parameter(pm => pm.PIN_NUM,            "PIN_NUM")
                                                        .Parameter(pm => pm.BENEFIT_MM,         "BENEFIT_MM")
                                                        .Parameter(pm => pm.HISTORY_SEQ_NUM,    "HISTORY_SEQ_NUM")
                                                        .Parameter(pm => pm.CLOCK_TYPE_CD,      "CLOCK_TYPE_CD")
                                                        .Parameter(pm => pm.CRE_TRAN_CD,        "CRE_TRAN_CD")
                                                        .Parameter(pm => pm.FED_CLOCK_IND,      "FED_CLOCK_IND")
                                                        .Parameter(pm => pm.FED_CMP_MTH_NUM,    "FED_CMP_MTH_NUM")
                                                        .Parameter(pm => pm.FED_MAX_MTH_NUM,    "FED_MAX_MTH_NUM")
                                                        .Parameter(pm => pm.HISTORY_CD,         "HISTORY_CD")
                                                        .Parameter(pm => pm.OT_CMP_MTH_NUM,     "OT_CMP_MTH_NUM")
                                                        .Parameter(pm => pm.OVERRIDE_REASON_CD, "OVERRIDE_REASON_CD")
                                                        .Parameter(pm => pm.TOT_CMP_MTH_NUM,    "TOT_CMP_MTH_NUM")
                                                        .Parameter(pm => pm.TOT_MAX_MTH_NUM,    "TOT_MAX_MTH_NUM")
                                                        .Parameter(pm => pm.UPDATED_DT,         "UPDATED_DT")
                                                        .Parameter(pm => pm.USER_ID,            "USER_ID")
                                                        .Parameter(pm => pm.WW_CMP_MTH_NUM,     "WW_CMP_MTH_NUM")
                                                        .Parameter(pm => pm.WW_MAX_MTH_NUM,     "WW_MAX_MTH_NUM")
                                                        .Parameter(pm => pm.COMMENT_TXT,        "COMMENT_TXT")
                                                        .Result(rs => rs.Id, "Id"))
                                        .Update(sp => sp.HasName("DB2_T0459_Update")
                                                        .Parameter(pm => pm.Id, "Id")
                                                        .Parameter(pm => pm.PIN_NUM,            "PIN_NUM")
                                                        .Parameter(pm => pm.BENEFIT_MM,         "BENEFIT_MM")
                                                        .Parameter(pm => pm.HISTORY_SEQ_NUM,    "HISTORY_SEQ_NUM")
                                                        .Parameter(pm => pm.CLOCK_TYPE_CD,      "CLOCK_TYPE_CD")
                                                        .Parameter(pm => pm.CRE_TRAN_CD,        "CRE_TRAN_CD")
                                                        .Parameter(pm => pm.FED_CLOCK_IND,      "FED_CLOCK_IND")
                                                        .Parameter(pm => pm.FED_CMP_MTH_NUM,    "FED_CMP_MTH_NUM")
                                                        .Parameter(pm => pm.FED_MAX_MTH_NUM,    "FED_MAX_MTH_NUM")
                                                        .Parameter(pm => pm.HISTORY_CD,         "HISTORY_CD")
                                                        .Parameter(pm => pm.OT_CMP_MTH_NUM,     "OT_CMP_MTH_NUM")
                                                        .Parameter(pm => pm.OVERRIDE_REASON_CD, "OVERRIDE_REASON_CD")
                                                        .Parameter(pm => pm.TOT_CMP_MTH_NUM,    "TOT_CMP_MTH_NUM")
                                                        .Parameter(pm => pm.TOT_MAX_MTH_NUM,    "TOT_MAX_MTH_NUM")
                                                        .Parameter(pm => pm.UPDATED_DT,         "UPDATED_DT")
                                                        .Parameter(pm => pm.USER_ID,            "USER_ID")
                                                        .Parameter(pm => pm.WW_CMP_MTH_NUM,     "WW_CMP_MTH_NUM")
                                                        .Parameter(pm => pm.WW_MAX_MTH_NUM,     "WW_MAX_MTH_NUM")
                                                        .Parameter(pm => pm.COMMENT_TXT,        "COMMENT_TXT")));

            #endregion
        }
    }
}
